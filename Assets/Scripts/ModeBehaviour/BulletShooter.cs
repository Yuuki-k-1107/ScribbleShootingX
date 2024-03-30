using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    private shootPlayer shoot;
    private int count = 0;
    private const int MAX_COUNT = 50;
    private void Initialize()
    {
        shoot = GameObject.Find("Triangle").GetComponent<shootPlayer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.At))
        {
            Debug();
        }
        Fire();
    }

    private void Fire()
    {
        if (shoot == null) return;
        if (shoot.mode != 2) return;
        if (bulletPrefab == null) return;
        else
        {
            count = 0;
            Quaternion rotation = this.transform.rotation;
            GameObject bullet = Instantiate(bulletPrefab, this.transform.position, rotation);
            BulletBehavior bb = bullet.GetComponent<BulletBehavior>();
            bullet.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
            bb.speed = 3.0f;
            bb.maxSpeed = 3.0f;
            bb.power = 1;
            bb.isMode0 = true;
        }
    }

    private void Debug()
    {
        if (shoot != null)
        {
            UnityEngine.Debug.Log("shootPlayer is collectly set");
        }
        else
        {
            UnityEngine.Debug.LogWarning("shootPlayer is missing");
        }
    }
}
