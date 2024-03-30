using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
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
    protected Rigidbody2D rb;
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
    protected int started = 0;
    public ShootingScript ss = null;
    protected int i = 0;
    protected int j = 0;
    public int index = 0;
    protected List<Vector2> trajectory;
    protected List<Vector2> list;
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
    protected Attribute attribute = Attribute.Normal;

    // 軌跡用
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
        Invoke("DestroyObject", destructionDelay); // destructionDelay秒後にDestroyObject関数を呼び出す
        rb = GetComponent<Rigidbody2D>();
        speed = initialSpeed;
        ss = GameObject.Find("Triangle").GetComponent<ShootingScript>();
        SetAttribute(Attribute.Normal);
    }

    protected void Initialize(Attribute attribute)
    {
        Invoke("DestroyObject", destructionDelay); // destructionDelay秒後にDestroyObject関数を呼び出す
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
            // 軌跡の最終到達点
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
                speed = maxSpeed; //maxSpeedを超えないようにする
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
            // destructionTimeフレーム経過後に弾を消す
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
        float rot = mb.getRotation(); //自機の回転を取得
        float sp = mb.getSpeed(); // 自機のスピードを取得
        this.transform.Rotate(0f, 0f, rot * ratio); // 弾に回転を加える
        this.rb.velocity = transform.up * (speed + ratio * sp);
    }

    protected virtual void DestroyObject()
    {
        Destroy(gameObject); // このスクリプトがアタッチされているゲームオブジェクトを破壊
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
