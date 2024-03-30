using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class BulletBehaviorCore : MonoBehaviour
{
    // �e�̎���
    public float destructionDelay = 3.0f;
    // 1.����G�l���M�[
    public int energy = 5;
    // 2.�^����_���[�W
    public int power = 5;
    // 3.���E����₷��
    public int counteraction = 0;
    // 4.�e��
    public float speed = 0; // ���݂̑��x
    public float initialSpeed = 6.0f; // ����
    public float maxSpeed = 12.0f; // �ō���
    // 5.�A�ˑ��x
    public float coolDown = 0.5f;
    // 6.�e�̉����x
    public float accel = 3.0f;
    // Rigidbody2D
    private Rigidbody2D rb;
    // Mode�Ɋւ���bool�ϐ�
    // Mode0 : �ʏ�̒e�E���Ԍo�߂ŏ������Ȃ�
    // Mode1 : �e�Ɏ��@�̊�����������
    // Mode2 : �e����Q���ɂł���
    // Mode3 : �e�̃N�[���_�E�����x���������Ȃ�
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
    // �����Ɋւ����
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

    // �O�՗p
    private int count = 0;
    private int currentIdx = 0;
    void Start()
    {
        Invoke("DestroyObject", destructionDelay); // destructionDelay�b���DestroyObject�֐����Ăяo��
        rb = GetComponent<Rigidbody2D>();
        speed = initialSpeed;
        ss = GameObject.Find("Triangle").GetComponent<ShootingScript>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        //�G�̒e�Ȃ�
        if (collider2D.gameObject.CompareTag("EnemyBullet") || collider2D.gameObject.CompareTag("Bullet"))
        {
            // �����̒e�Ȃ瑊�E���Ȃ�
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
        #region �f�o�b�O(����)
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
            // destructionTime�t���[���o�ߌ�ɒe������
            if (delayedDestroy && ((time - started) > destructionTime))
            {
                Destroy(this.gameObject);
            }
            else
            {
                return;
            }
        }
        if (isMode0) // mode0�͒e������������
        {
            Vector3 size = new Vector3(this.transform.localScale.x * 0.998f, this.transform.localScale.y * 0.998f, this.transform.localScale.z * 0.998f);
            this.transform.localScale = size;
        }
        if (speed < maxSpeed)
        {
            speed += accel * Time.deltaTime;
            if (speed > maxSpeed)
            {
                speed = maxSpeed; //maxSpeed�𒴂��Ȃ��悤�ɂ���
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
            #region �v
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
                        // �ŏ��̗v�f�Ȃ�Η�x�N�g���𒼋߂̃x�N�g���Ƃ���
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
        float rot = mb.getRotation(); //���@�̉�]���擾
        float sp = mb.getSpeed(); // ���@�̃X�s�[�h���擾
        this.transform.Rotate(0f, 0f, rot * ratio); // �e�ɉ�]��������
        this.rb.velocity = transform.up * (speed + ratio * sp);
    }

    void DestroyObject()
    {
        if (this.attribute.Equals(Attribute.Bomb))
        // �e�̑��������e�̂Ƃ�
        {
            // ���S�ʒu�����݂̃I�u�W�F�N�g�̈ʒu�Ƃ��A���a��2�Ƃ��ĉ~�͈͓̔��ɂ���R���C�_�[���擾
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2.0f);

            // �͈͓��̂��ׂẴR���C�_�[�ɑ΂��ď������s��
            foreach (Collider2D collider in colliders)
            {
                // �������g�̃R���C�_�[�𖳎�����
                if (collider.gameObject != this.gameObject)
                {
                    EnemyBehavior eb = collider.gameObject.GetComponent<EnemyBehavior>();
                    if (eb != null)
                    {
                        // �G�Ƀ_���[�W
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
            Destroy(gameObject); // ���̃X�N���v�g���A�^�b�`����Ă���Q�[���I�u�W�F�N�g��j��
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
