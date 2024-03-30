using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUBaseBehaviour : machineBehaviour
{
    public override Vector3 makePos(){
        return mousePos;
    }
    public void CPUFire(int type)
    {
        //GameObject bullet = Instantiate(bulletPrefab[type],transform.position, transform.rotation);
        //BulletBehavior bb = bullet.GetComponent<BulletBehavior>();
        //bb.gameObject.tag = "EnemyBullet";
    }
}
