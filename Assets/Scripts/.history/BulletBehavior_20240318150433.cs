using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
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
    protected Rigidbody2D rb;
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
    protected int started = 0;
    public ShootingScript ss = null;
    protected int i = 0;
    protected int j = 0;
    public int index = 0;
    protected List<Vector2> trajectory;
    protected List<Vector2> list;
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
    protected Attribute attribute = Attribute.Normal;

    // �O�՗p
    protected int count = 0;
    protected int currentIdx = 0;
    private void Start()
    {
        Initialize();
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        ColliderRule(collider2D);
    }

    protected void Initialize()
    {
        Invoke("DestroyObject", destructionDelay); // destructionDelay�b���DestroyObject�֐����Ăяo��
        rb = GetComponent<Rigidbody2D>();
        speed = initialSpeed;
        ss = GameObject.Find("Triangle").GetComponent<ShootingScript>();
        SetAttribute(Attribute.Normal);
    }

    protected void Initialize(Attribute attribute)
    {
        Invoke("DestroyObject", destructionDelay); // destructionDelay�b���DestroyObject�֐����Ăяo��
        rb = GetComponent<Rigidbody2D>();
        speed = initialSpeed;
        ss = GameObject.Find("Triangle").GetComponent<ShootingScript>();
        SetAttribute(attribute);
    }

    private void Update()
    {
        // ---------------------------
        if (DelayedDestroy()) return;
        Shrink(isMode0);
        Accelerate();
        // ---------------------------
        if (isDrawn)
        {
            PresetTrajectorialMove();
        }
        else if (isTrajected)
        {
            DraggedTrajectorialMove();
        }
        else
        {
            NormalMove();
        }
    }

    protected void SetParam(float destructionDelay, int energy, int power, int counteraction, float initialSpeed, float maxSpeed, float coolDown, float accel)
    {
        this.destructionDelay = destructionDelay;
        this.energy = energy;
        this.power = power;
        this.counteraction = counteraction;
        this.speed = initialSpeed;
        this.initialSpeed = initialSpeed;
        this.maxSpeed = maxSpeed;
        this.coolDown = coolDown;
        this.accel = accel;
    }

    protected void ColliderRule(Collider2D collider2D)
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
    }


    protected void PresetTrajectorialMove()
    {
        while (list == null)
        {
            print("waiting for receiving list...");
        }
        MoveInTrajectory();
        if (isMode1)
        {
            AddInertia();
        }
    }

    protected void DraggedTrajectorialMove()
    {
        if (index == i)
        {
            // �O�Ղ̍ŏI���B�_
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
                    i++;
                }
            }
            else
            {
                j++;
            }
        }
    }

    protected void NormalMove()
    {
        if (isMode1)
        {
            AddInertia();
        }
        else
        {
            rb.velocity = transform.up * speed;
        }
    }

    protected void Accelerate()
    {
        if (speed < maxSpeed)
        {
            speed += accel * Time.deltaTime;
            if (speed > maxSpeed)
            {
                speed = maxSpeed; //maxSpeed�𒴂��Ȃ��悤�ɂ���
            }
        }
    }

    protected void Shrink(bool isMode0)
    {
        if (!isMode0) return;
        Vector3 size = new Vector3(this.transform.localScale.x * 0.998f, this.transform.localScale.y * 0.998f, this.transform.localScale.z * 0.998f);
        this.transform.localScale = size;
    }

    protected bool DelayedDestroy()
    {
        if (delayedDestroy)
        {
            int destructionTime = 300;
            int time = Time.frameCount;
            this.rb.velocity = Vector2.zero;
            // destructionTime�t���[���o�ߌ�ɒe������
            if (delayedDestroy && ((time - started) > destructionTime))
            {
                Destroy(this.gameObject);
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    // Add inertia that is related to machine
    protected void AddInertia()
    {
        float ratio = 0.2f;
        mainBehaviour mb = GameObject.Find("Triangle").GetComponent<mainBehaviour>();
        float rot = mb.getRotation(); //���@�̉�]���擾
        float sp = mb.getSpeed(); // ���@�̃X�s�[�h���擾
        this.transform.Rotate(0f, 0f, rot * ratio); // �e�ɉ�]��������
        this.rb.velocity = transform.up * (speed + ratio * sp);
    }

    protected virtual void DestroyObject()
    {
        Destroy(gameObject); // ���̃X�N���v�g���A�^�b�`����Ă���Q�[���I�u�W�F�N�g��j��
    }

    protected void MoveInTrajectory()
    {
        int maxCount = 20;
        if (count < maxCount) {
            count++;
            return; 
        }
        count = 0;
        if(list != null)
        {
            if (currentIdx < list.Count)
            {
                if (currentIdx == 0)
                {
                    this.rb.velocity += speed * list[currentIdx];
                }
                else
                {
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

    public void SetAttribute(Attribute attribute)
    {
        this.attribute = attribute;
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
