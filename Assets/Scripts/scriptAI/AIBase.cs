using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIBase : MonoBehaviour
{
    private GameObject player;
    private mainBehaviour mb;
    private Rigidbody rigid;
    private CPUBaseBehaviour cpu;
    // こちらが撃った弾を保存するmap
    private IDictionary<int, GameObject> map = new Dictionary<int, GameObject>();
    // cpuが撃つ弾の番号
    public int bulletNumber = 0;
    // 移動方向と大きさ
    Vector2 movement;
    void Start()
    {
        player = GameObject.Find("Triangle");
        mb = player.GetComponent<mainBehaviour>();
        rigid = player.GetComponent<Rigidbody>();
        cpu = this.gameObject.GetComponent<CPUBaseBehaviour>();
    }

    void Update()
    {
        cpu.mousePos = movement;
        //cpu.CPUFire(bulletNumber);
    }

    public virtual void move(){

    }

    public virtual void fire(){


    }
    // プレイヤーの位置を取得
    public Vector2 getPlayerPosition()
    {
        return player.transform.position;
    }

    public Vector2 getPlayerSpeed()
    {
        return new Vector2(rigid.velocity.x, rigid.velocity.y);
    }

    public Vector3 getPlayerRotation()
    {
        return player.transform.localEulerAngles;
    }

    public int getPlayerEnergy()
    {
        return mb.energy;
    }
}
