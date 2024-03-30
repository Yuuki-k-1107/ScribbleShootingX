using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Apple;
using static UnityEngine.GraphicsBuffer;

public class CPUBehaviour : MonoBehaviour
{
    public GameObject[] bulletPrefab = new GameObject[4];
    public int cpuHP = 100;
    [SerializeField] int shootingInterval = 135;
    private int counter = 0;
    private float elapsedTime = 0f;
    public static bool isVsCPU = false;
    private bool isMoving = false;
    private bool isLeft = false;
    private Vector3 destination = new Vector3(-1f, 8f, 0f);
    private GameObject player;
    private Transform playerTransform;
    void Start()
    {
        this.gameObject.SetActive(isVsCPU);
        this.transform.Rotate(0,0,180);
        this.transform.position = destination;
        player = GameObject.Find("Triangle");
        playerTransform = player.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet")) { 
            BulletBehavior bb = collision.gameObject.GetComponent<BulletBehavior>();
            if (bb != null)
            {
                this.cpuHP -= bb.power;
                Destroy(bb.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (shootingInterval > counter) counter++;
        else
        {
            counter = 0;
            CPUFire();
        }
        // 移動用のスクリプト
        // 具体的にはプレーヤーを追尾するつもり
        Move();
        LookAtPlayer(playerTransform);
        //print(player.transform.rotation);
        //this.transform.RotateAround(playerTransform.position, Vector3.forward, 15.0f * Time.deltaTime);
        //this.transform.rotation *= Quaternion.AngleAxis(10f*Time.deltaTime, Vector3.forward);
        if (this.cpuHP <= 0)
        {
            Defeated();
        }
    }

    private void Move()
    {
        Vector3 newPos = this.transform.position;
        Vector2 direction = new Vector2(playerTransform.position.x, playerTransform.position.y) - (Vector2)newPos;
        float distance = direction.magnitude;
        Vector2 moveDirection = direction.normalized;
        if (distance > 1.8f) this.transform.Translate(moveDirection * -3f * Time.deltaTime);
        else this.transform.Translate(moveDirection * 3f * Time.deltaTime);
        if (this.transform.position.x < -13.0f) this.transform.position = new Vector3(-13.0f, this.transform.position.y, 0f);
        if (this.transform.position.x > 13.0f) this.transform.position = new Vector3(13.0f, this.transform.position.y, 0f);
        if (this.transform.position.y < -10.0f) this.transform.position = new Vector3(this.transform.position.x, -10.0f, 0f);
        if (this.transform.position.y > 10.0f) this.transform.position = new Vector3(this.transform.position.x, 10.0f, 0f);
    }

    void CPUFire()
    {
        int bulletID = UnityEngine.Random.Range(0,4);
        GameObject bullet = Instantiate(bulletPrefab[bulletID],transform.position, transform.rotation);
        BulletBehavior bb = bullet.GetComponent<BulletBehavior>();
        bb.gameObject.tag = "EnemyBullet";
    }

    void Defeated()
    {
        Debug.Log("You Win!!");
        Destroy(this.gameObject);
    }

    private void LookAtPlayer(Transform playerTransform)
    {
        /*        Vector2 direction = playerTransform.position - this.transform.position;
                float angle = MathF.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0,0,angle);
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, 2.0f * Time.deltaTime);*/
        // 対象物へのベクトルを算出
        Vector3 toDirection = playerTransform.position - this.transform.position;
        // 対象物へ回転する
        this.transform.rotation = Quaternion.FromToRotation(Vector3.up, toDirection);
    }
}
