using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyBehavior : MonoBehaviour
{
    public int attack = 10;
    public int HP = 100;
    public float xSpeed = 15f;
    public float ySpeed = 5f;
    public bool isStuck = false;
    public GameObject hitBullet = null;
    public int hitBulletId = 0;
    // 移動用の時間変数
    private int t = 0;
    private void OnTriggerEnter2D(Collider2D something)
    {
        // 衝突したオブジェクトのタグが "bullet" の場合
        if (something.gameObject.CompareTag("Bullet"))
        {
            BulletBehavior bb = something.gameObject.GetComponent<BulletBehavior>();
            if (bb != null)
            {
                if (!bb.GetAttribute().Equals(bb.GetAttribute("Rock")))
                {
                    // 体力が弾の威力分減る
                    HP -= bb.power;
                    // スコアを威力の半分加算
                    AddScore((int)bb.power / 2);
                    // 弾を破壊
                    Destroy(bb.gameObject);
                }
            }
            else
            {
                // デバッグ用; bbがない時
                Console.Error.WriteLine("Bullet Behavior is null!!");
            }
            Console.Write(HP);
        }
        // プレーヤーと接触
        else if (something.gameObject.CompareTag("Player"))
        {
            mainBehaviour mb = something.gameObject.GetComponent<mainBehaviour>();
            // プレーヤーのHPがattackだけ減る
            mb.myHP -= attack;
            // 敵自身を破壊し、残りHPの0.1倍の得点
            Die((int)(this.HP / 10));
        }
        else
        {
            // デバッグ用; 接触の判定
            Console.Write("Something collided");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            BulletBehavior bb = collision.gameObject.GetComponent<BulletBehavior>();
            if (bb != null)
            {
                if (bb.GetAttribute().Equals(bb.GetAttribute("Rock")))
                {
                    hitBulletId = collision.gameObject.GetInstanceID();
                    hitBullet = collision.gameObject;
                    print($"hitBullet ID {hitBulletId}, {collision.gameObject.GetInstanceID()} is set to Enemy ID:{this.gameObject.GetInstanceID()}");
                    bb.SetStarted();
                    bb.delayedDestroy = true;
                    isStuck = true;
                }
            }
            else
            {
                // デバッグ用; bbがない時
                Console.Error.WriteLine("Bullet Behavior is null!!");
            }
            Console.Write(HP);
        }
    }

    void Update()
    {
        // 死亡
        if (HP <= 0) Die(100);
        // 移動
        Move();
        // 画面外に出た場合は
        if (Mathf.Abs(this.transform.position.x) >= 14.0f || this.transform.position.y < -11.0f)
        {
            // ペナルティ-100点
            Die(-100);
        }
        if (hitBullet != null) {
            if (GameObjectExistsWithInstanceID(hitBulletId))
            {
                isStuck = true;
            }else
            {
                hitBullet = null;
                isStuck = false;
            }
        } else
        {
            isStuck = false;
        }
    }

    private void Move()
    {
        // 横移動を正弦関数で表現
        float x = xSpeed * Mathf.Sin(Mathf.PI * t / 50);
        // 移動させる
        Vector3 speedVec = new Vector3(x, -ySpeed, 0);
        if(!isStuck) this.transform.Translate(speedVec * Time.deltaTime);
        t++;
    }

    private void Die(int score)
    {
        HP = 0;
        AddScore(score);
        Destroy(gameObject);
    }

    // スコア加算
    private void AddScore(int amount)
    {
        ScoreManager.ChangeScore(amount);
    }

    // 指定されたインスタンスIDに対応するゲームオブジェクトが存在するかどうかを判定する関数
    public bool GameObjectExistsWithInstanceID(int instanceID)
    {
        // インスタンスIDに対応するゲームオブジェクトを検索し、存在するかどうかを返す
        GameObject gameObject = GameObject.FindObjectsOfType<GameObject>().FirstOrDefault(go => go.GetInstanceID() == instanceID);
        return gameObject != null;
    }
}
