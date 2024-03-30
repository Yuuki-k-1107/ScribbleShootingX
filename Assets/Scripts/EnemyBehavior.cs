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
    // �ړ��p�̎��ԕϐ�
    private int t = 0;
    private void OnTriggerEnter2D(Collider2D something)
    {
        // �Փ˂����I�u�W�F�N�g�̃^�O�� "bullet" �̏ꍇ
        if (something.gameObject.CompareTag("Bullet"))
        {
            BulletBehavior bb = something.gameObject.GetComponent<BulletBehavior>();
            if (bb != null)
            {
                if (!bb.GetAttribute().Equals(bb.GetAttribute("Rock")))
                {
                    // �̗͂��e�̈З͕�����
                    HP -= bb.power;
                    // �X�R�A���З͂̔������Z
                    AddScore((int)bb.power / 2);
                    // �e��j��
                    Destroy(bb.gameObject);
                }
            }
            else
            {
                // �f�o�b�O�p; bb���Ȃ���
                Console.Error.WriteLine("Bullet Behavior is null!!");
            }
            Console.Write(HP);
        }
        // �v���[���[�ƐڐG
        else if (something.gameObject.CompareTag("Player"))
        {
            mainBehaviour mb = something.gameObject.GetComponent<mainBehaviour>();
            // �v���[���[��HP��attack��������
            mb.myHP -= attack;
            // �G���g��j�󂵁A�c��HP��0.1�{�̓��_
            Die((int)(this.HP / 10));
        }
        else
        {
            // �f�o�b�O�p; �ڐG�̔���
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
                // �f�o�b�O�p; bb���Ȃ���
                Console.Error.WriteLine("Bullet Behavior is null!!");
            }
            Console.Write(HP);
        }
    }

    void Update()
    {
        // ���S
        if (HP <= 0) Die(100);
        // �ړ�
        Move();
        // ��ʊO�ɏo���ꍇ��
        if (Mathf.Abs(this.transform.position.x) >= 14.0f || this.transform.position.y < -11.0f)
        {
            // �y�i���e�B-100�_
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
        // ���ړ��𐳌��֐��ŕ\��
        float x = xSpeed * Mathf.Sin(Mathf.PI * t / 50);
        // �ړ�������
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

    // �X�R�A���Z
    private void AddScore(int amount)
    {
        ScoreManager.ChangeScore(amount);
    }

    // �w�肳�ꂽ�C���X�^���XID�ɑΉ�����Q�[���I�u�W�F�N�g�����݂��邩�ǂ����𔻒肷��֐�
    public bool GameObjectExistsWithInstanceID(int instanceID)
    {
        // �C���X�^���XID�ɑΉ�����Q�[���I�u�W�F�N�g���������A���݂��邩�ǂ�����Ԃ�
        GameObject gameObject = GameObject.FindObjectsOfType<GameObject>().FirstOrDefault(go => go.GetInstanceID() == instanceID);
        return gameObject != null;
    }
}
