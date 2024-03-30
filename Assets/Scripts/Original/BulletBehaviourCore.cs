using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class BulletBehaviorCore : MonoBehaviour
{
    // 弾の寿命
    public float destructionDelay = 3.0f;
    // 1.消費エネルギー
    public int energy = 5;
    // 2.与えるダメージ
    public int power = 5;
    // 3.相殺されやすさ
    public int counteraction = 0;
    // 4.弾速
    public float speed = 0; // 現在の速度
    public float initialSpeed = 6.0f; // 初速
    public float maxSpeed = 12.0f; // 最高速
    // 5.連射速度
    public float coolDown = 0.5f;
    // 6.弾の加速度
    public float accel = 3.0f;
    // Rigidbody2D
    private Rigidbody2D rb;
    // Modeに関するbool変数
    // Mode0 : 通常の弾・時間経過で小さくなる
    // Mode1 : 弾に自機の慣性を加える
    // Mode2 : 弾を障害物にできる
    // Mode3 : 弾のクールダウン速度が小さくなる
    public bool isMode0 = false;
    public bool isMode1 = false;
    public bool isMode2 = false;
    public bool isCPU = false;
    public bool isTrajected = false;
    public bool isDrawn = false;
    public bool delayedDestroy = false;
    private int started = 0;
    public ShootingScript ss = null;
    private int i = 0;
    private int j = 0;
    public int index = 0;
    private List<Vector2> trajectory;
    private List<Vector2> list;
    // 属性に関する列挙
    public enum Attribute
    {
        Normal,
        Fire,
        Ice,
        Rock,
        Storm,
        Lead,
        Bomb,
        Cannon
    }
    private Attribute attribute = Attribute.Normal;

    // 軌跡用
    private int count = 0;
    private int currentIdx = 0;
    void Start()
    {
        Invoke("DestroyObject", destructionDelay); // destructionDelay秒後にDestroyObject関数を呼び出す
        rb = GetComponent<Rigidbody2D>();
        speed = initialSpeed;
        ss = GameObject.Find("Triangle").GetComponent<ShootingScript>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        //敵の弾なら
        if (collider2D.gameObject.CompareTag("EnemyBullet") || collider2D.gameObject.CompareTag("Bullet"))
        {
            // 味方の弾なら相殺しない
            if (!this.gameObject.tag.Equals(collider2D.gameObject.tag))
            {
                if (this.counteraction == collider2D.gameObject.GetComponent<BulletBehavior>().counteraction)
                {
                    Destroy(collider2D.gameObject);
                    this.DestroyObject();
                }
                else if (this.counteraction > collider2D.gameObject.GetComponent<BulletBehavior>().counteraction)
                {
                    Destroy(collider2D.gameObject);
                }
                else
                {
                    this.DestroyObject();
                }
            }
        }

        // Console.Write("hello");
        //DestroyObject();
    }
    void Update()
    {
        #region デバッグ(属性)
        if (Input.GetKeyDown(KeyCode.A))
        {
            print($"Attribute : {attribute}");
        }
        #endregion
        // ---------------------------
        SetAttribute(this.gameObject.name);

        if (delayedDestroy)
        {
            int destructionTime = 300;
            int time = Time.frameCount;
            this.rb.velocity = Vector2.zero;
            // destructionTimeフレーム経過後に弾を消す
            if (delayedDestroy && ((time - started) > destructionTime))
            {
                Destroy(this.gameObject);
            }
            else
            {
                return;
            }
        }
        if (isMode0) // mode0は弾を小さくする
        {
            Vector3 size = new Vector3(this.transform.localScale.x * 0.998f, this.transform.localScale.y * 0.998f, this.transform.localScale.z * 0.998f);
            this.transform.localScale = size;
        }
        if (speed < maxSpeed)
        {
            speed += accel * Time.deltaTime;
            if (speed > maxSpeed)
            {
                speed = maxSpeed; //maxSpeedを超えないようにする
            }
        }
        if (isDrawn)
        {
            Vector2 old = new Vector2(0, 0);
            float oldSpeed = 0.0f;
            while (list == null)
            {
                print("waiting for receiving list...");
            }
            MoveInTrajectory();
            if (isMode1)
            {
                AddInertia();
            }
            #region 没
            /*foreach(Vector2 move in list)
            {
                if (list.IndexOf(move) == 0)
                {
                    for (int i = 0; i < 100000; i++)
                    {
                        this.rb.velocity += speed * move;
                    }
                }
                else
                {
                    for (int i = 0; i < 100000; i++)
                    {
                        this.rb.velocity += (speed  * move - oldSpeed * old);
                    }
                }
                old = move;
                oldSpeed = speed;
            }*/
            #endregion
        }
        else if (isTrajected)
        {
            if (index == i)
            {
                i = 0;
                isTrajected = false;
            }
            else
            {
                if (j == 2)
                {
                    j = 0;
                    if (true)
                    {
                        // 最初の要素ならば零ベクトルを直近のベクトルとする
                        Vector2 previous = (i == 0) ? new Vector2(0f, 0f) : this.trajectory[i - 1];
                        this.rb.velocity = (this.trajectory[i] - previous) * 5.0f * speed;
                        //this.transform.Translate(this.trajectory[i] - previous);
                        i++;
                    }
                }
                else
                {
                    j++;
                }
            }
        }
        else
        {
            if (isMode1)
            {
                //Debug.Log("Mode1 called");
                AddInertia();
            }
            else
            {
                rb.velocity = transform.up * speed;
            }
        }
        if (/*isMode2*/ attribute.Equals(Attribute.Rock))
        {
            destructionDelay = 0.5f;
        }

    }
    // Add inertia that is related to machine
    private void AddInertia()
    {
        float ratio = 0.2f;
        mainBehaviour mb = GameObject.Find("Triangle").GetComponent<mainBehaviour>();
        float rot = mb.getRotation(); //自機の回転を取得
        float sp = mb.getSpeed(); // 自機のスピードを取得
        this.transform.Rotate(0f, 0f, rot * ratio); // 弾に回転を加える
        this.rb.velocity = transform.up * (speed + ratio * sp);
    }

    void DestroyObject()
    {
        if (this.attribute.Equals(Attribute.Bomb))
        // 弾の属性が爆弾のとき
        {
            // 中心位置を現在のオブジェクトの位置とし、半径を2として円の範囲内にあるコライダーを取得
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2.0f);

            // 範囲内のすべてのコライダーに対して処理を行う
            foreach (Collider2D collider in colliders)
            {
                // 自分自身のコライダーを無視する
                if (collider.gameObject != this.gameObject)
                {
                    EnemyBehavior eb = collider.gameObject.GetComponent<EnemyBehavior>();
                    if (eb != null)
                    {
                        // 敵にダメージ
                        eb.HP -= this.power;
                    }
                }
            }
        }
        if (/*isMode2*/ attribute.Equals(Attribute.Rock))
        {
            this.GetComponent<Rigidbody2D>().isKinematic = true;
            this.GetComponent<Rigidbody2D>().freezeRotation = true;
            SetStarted();
            delayedDestroy = true;
        }
        else
        {
            Destroy(gameObject); // このスクリプトがアタッチされているゲームオブジェクトを破壊
        }
    }

    private void MoveInTrajectory()
    {
        int maxCount = 20;
        if (count < maxCount)
        {
            count++;
            return;
        }
        count = 0;
        if (list != null)
        {
            if (currentIdx < list.Count)
            {
                if (currentIdx == 0)
                {
                    this.rb.velocity += speed * list[currentIdx];
                }
                else
                {
                    //this.rb.velocity += (speed * new Vector2(list[currentIdx].y, list[currentIdx].x) - speed * new Vector2(list[currentIdx-1].y, list[currentIdx-1].x));
                    this.rb.velocity += (speed * list[currentIdx] - speed * list[currentIdx - 1]);
                }
            }
            else
            {
                currentIdx = 0;
                list.Clear();
                isDrawn = false;
            }
        }
        currentIdx++;
    }

    public void SetTrajectory(List<Vector2> vectors)
    {
        this.trajectory = vectors;
        index = vectors.Count;
    }

    public void SetList(List<Vector2> list)
    {
        this.list = list;
    }

    public Attribute GetAttribute()
    {
        return attribute;
    }

    public Attribute GetAttribute(string attributeName)
    {
        return (Attribute)Enum.Parse(typeof(Attribute), attributeName, true);
    }

    public void SetStarted()
    {
        started = Time.frameCount;
    }

    public void SetAttribute(string bulletName)
    {
        if (bulletName.StartsWith("Fire"))
        {
            this.attribute = Attribute.Fire;
        }
        else if (bulletName.StartsWith("Ice"))
        {
            this.attribute = Attribute.Ice;
        }
        else if (bulletName.StartsWith("Rock"))
        {
            this.attribute = Attribute.Rock;
        }
        else if (bulletName.StartsWith("Storm"))
        {
            this.attribute = Attribute.Storm;
        }
        else if (bulletName.StartsWith("Lead"))
        {
            this.attribute = Attribute.Lead;
        }
        else if (bulletName.StartsWith("Bomb"))
        {
            this.attribute = Attribute.Bomb;
        }
        else if (bulletName.StartsWith("Cannon"))
        {
            this.attribute = Attribute.Cannon;
        }
        else
        {
            this.attribute = Attribute.Normal;
        }
    }
}
