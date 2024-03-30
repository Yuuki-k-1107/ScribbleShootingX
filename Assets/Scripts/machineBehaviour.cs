using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class machineBehaviour : MonoBehaviour
{
    //最大速度
    [SerializeField] float maxSpeed = 20.0f;
    //加速度
    [SerializeField] float acceleration = 10.0f;
    //減速するときの加速度
    [SerializeField] float deceleration = 10.0f;
    //速度
    [SerializeField] float speed = 0f;
	//回転速度
	[SerializeField] float rSpeed = 360.0f;
    // エネルギー
    public const int MAXENERGY = 500;
    public int energy = 0;
    // エネルギー回復の単位時間
    [SerializeField] int energyRecovery = 400;
    //これはエネルギーが1回復するまでのカウンタ
    private int recoveryCooldown = 0;
    //移動判定用の変数
    bool isMoving;

    public Vector3 mousePos, worldPos;

    // 体力
    public int myHP = 100;
    public int attack = 10;
    [SerializeField] float rotation;

    // Start is called before the first frame update
    void Start()
    {
        energy = MAXENERGY;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            BulletBehavior bb = collision.gameObject.GetComponent<BulletBehavior>();
            if (bb != null)
            {
                myHP -= bb.power;
                Destroy(bb.gameObject);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        mousePos = makePos();
        worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
        // エネルギー回復
        if (!PauseManager.isPaused)
        {
            recoveryCooldown++;
            if (recoveryCooldown >= energyRecovery)
            {
                recoveryCooldown = 0;
                if (energy < MAXENERGY)
                {
                    energy++;
                }
            }
        }
        if(Vector3.Distance(transform.position, worldPos) <= speed * speed / 2 / deceleration){
            speed -= deceleration * Time.deltaTime;
            if(speed < 0){
                speed = 0; //0を下回らないようにする
            }
        }
        else if(speed < maxSpeed){
            speed += acceleration * Time.deltaTime;
            if(speed > maxSpeed){
                speed = maxSpeed; //maxSpeedを超えないようにする
            }
        }
		Vector3 tmpP = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, worldPos, speed * Time.deltaTime);
		
        //isMovingで動いたかどうか確認
        if(transform.position == tmpP){
			isMoving = false;
		}
		else{
			isMoving = true;
		}
		
        //動いたかどうか確認し、動いた時だけ回転
		if(isMoving){
            rotation = Vector2.SignedAngle(new Vector2(0, 1), new Vector2(worldPos[0]-transform.position[0], worldPos[1] - transform.position[1]));
            rotation -= transform.localEulerAngles.z;
            if(rotation > 180){
                rotation -= 360;
            }
            else if(rotation < -180){
                rotation += 360;
            }
            if(rotation > rSpeed * Time.deltaTime){
                rotation = rSpeed * Time.deltaTime;
            }
            else if(rotation < -rSpeed * Time.deltaTime){
                rotation = -rSpeed * Time.deltaTime;
            }
            
			transform.Rotate(0, 0, rotation);
		}
        else{
            speed = 0;
        }
    }

    public virtual Vector3 makePos(){
        return new Vector3(0f, 0f, 0f);
    }

    public float getRotation()
    {
        return this.rotation;
    }
    
    public float getSpeed()
    {
        return this.speed;
    }
}
