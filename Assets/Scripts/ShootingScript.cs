using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
#endif
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    //Constants
    public const int BULLET_NUM = 4;
    public const int MODE_NUM = 4;
    // -------------------- Public variables
    //Equipped bullet
    public GameObject[] bulletPrefab = new GameObject[BULLET_NUM];
    // Selected bullet
    public int bulletId = 0;
    // Cooling times for each BULLET_NUM bullet
    public float[] coolDownTime = new float[BULLET_NUM];
    // -------------------- private variables
    // The ratio for cooldown
    public float coolDownRatio = 1.0f;
    // The name of player
    public string myName;
    // Mode
    public int mode = 0;
    // flashing
    public int flash = 3;
    public int flashCount = 0;
    // List of trajectory
    public List<Vector2> trajectory = new List<Vector2>();
    public List<Vector2> temp = new List<Vector2>();
    // My position
    public Vector2 startPoint = new Vector2(0f, 0f);
    public dataManager dm = null;
    public saveData data = null;
    public virtual void Start()
    {
/*        myName = this.name;
        this.coolDownTime = new float[] { 0, 0, 0, 0 };
        dm = GameObject.Find("Empty").GetComponent<dataManager>();
        data = dm.Load($"{Application.dataPath}/data.json");
        print(dm);
        print(data);*/
    }
    public virtual void Update()
    {
        #region inherited by shootPlayer
        /*        if (Input.GetKeyDown(KeyCode.P))
                {
                    PrintMode();
                }
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    bulletId = 0;
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    bulletId = 1;
                }
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    bulletId = 2;
                }
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    bulletId = 3;
                }
                if (Input.GetKeyDown(KeyCode.M))
                {
                    mode += 1;
                    mode %= MODE_NUM;
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    List<Vector2> trajectoryData = data.trajectoryData[0];
                    if (trajectoryData != null)
                    {
                        Temporary(trajectoryData);
                    }
                    // When trajectoryData == null
                    else
                    {
                        print("No trajectory data");
                    }
                }
                // Shoot a bullet for preset trajectory
                if (Input.GetKeyDown(KeyCode.C))
                {
                    List<Vector2> trajectoryData = data.trajectoryData;
                    if (trajectoryData != null)
                    {
                        // Shrink trajectory in order to
                        float coef = 10f;
                        // # of division points between the points
                        int division = 7;
                        // Retain the position when C is pressed
                        Vector2 startPosition = Camera.main.ScreenToWorldPoint(this.transform.position);
                        Vector2 initial = new Vector2(0f, 0f);
                        Vector2 tempPoint = new Vector2(0f, 0f);
                        int idx = 0;
                        foreach (Vector2 point in trajectoryData)
                        {
                            #region rejected(there remains some Japanese comments)
                            *//*if (idx == 0)
                                // もし最初の座標なら
                            {
                                initial = point;
                                tempPoint = point;
                                temp.Add(startPosition / coef);
                            }
                            else
                            {
                                // 前の点と現在の点の中点を追加
                                temp.Add((((point + tempPoint) / 2) + startPosition - initial) / coef);

                                temp.Add((point + startPosition - initial) / coef);
                                // tempPoint を更新
                                tempPoint = point;
                            }
                            print($"index : {idx}");
                            idx++;*//*
                            #endregion
                            #region 採用案
                            if (idx == 0)
                            {
                                initial = point;
                                tempPoint = point;
                                temp.Add(new Vector2(0f, 0f));
                            }
                            else
                            {
                                if (division > 1)
                                {
                                    // add a vector such that the points divides i:(division-i)
                                    // ex. division = 5 & i = 3
                                    // [old]* . . [.] . *[new]
                                    for (int i = 1; i < division; i++)
                                    {
                                        float newx = (i * point.y + (division - i) * tempPoint.y) / (division * coef);
                                        float newy = (i * point.x + (division - i) * tempPoint.x) / (division * coef);
                                        temp.Add(new Vector2(newx, newy));
                                    }
                                }
                                #region division=2
                                // 前の点と現在の点の中点を追加
                                *//*                        float newx = (point.y + tempPoint.y) / (2 * coef);
                                                        float newy = (point.x + tempPoint.x) / (2 * coef);
                                                        temp.Add(new Vector2(newx,newy));*//*
                                #endregion
                                temp.Add(new Vector2(point.y / coef, point.x / coef));
                                // Refresh tempPoint
                                tempPoint = point;
                            }
                            print($"index : {idx}");
                            idx++;
                            #endregion
                        }
                        trajectory = temp.Select(item => item).ToList();
                        temp.Clear();
                        Fire(trajectory);
                    }

                }
                // Press Space/Z to shoot
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z))
                {
                    if (!myName.Equals("CPU"))
                    {
                        Fire();
                    }
                }*/
        #endregion
        // Click/Drag to shoot
        // 
        if (Input.GetMouseButtonDown(0))
        {
            startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            // regularize points in order to the initial point comes to (0, 0)
            temp.Add((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - startPoint);
        }
        if (Input.GetMouseButtonUp(0))
        {
            // Deepcopy the trajectory using LINQ
            trajectory = temp.Select(item => item).ToList();
            temp.Clear();
            Fire(trajectory);
        }
        for (int i = 0; i < BULLET_NUM; i++)
        {
            coolDownTime[i] += Time.deltaTime * coolDownRatio * 2.0f;
        }
    }

    protected void Temporary(List<Vector2> vectors)
    {
        if (!CanShoot()) { return; }
        Vector2 initial = (vectors[1] - vectors[0]).normalized;
        List<Vector2> list = new List<Vector2>();
        list.Add(initial);
        GameObject bullet = Instantiate(bulletPrefab[bulletId], this.transform.position, transform.rotation);
        BulletBehavior bb = bullet.GetComponent<BulletBehavior>();
        bb.isDrawn = true;
        for (int i = 2; i < vectors.Count; i++)
        {
            list.Add((vectors[i] - vectors[i - 1]).normalized);
        }
        bb.SetList(list);
        AfterShot(this.GetComponent<mainBehaviour>(), bulletPrefab[bulletId].GetComponent<BulletBehavior>());
    }

    protected virtual void Flash()
    {
        print($"Flash() left:{flash}, flashCount:{flashCount}");
        if (flash == 0) return;
        if (flashCount < 100) return;
        flash--;
        flashCount = 0;
        // Acquire the enemy objects
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            // Add 100 damages for each enemy
            enemy.GetComponent<EnemyBehavior>().HP -= 100;
        }
    }

    // Fire() when a key is pressed
    protected void Fire()
    {
        if (!CanShoot()) return;
        Quaternion bulletRotation = transform.rotation;
        GameObject bullet = Instantiate(bulletPrefab[bulletId], transform.position, bulletRotation);
        mainBehaviour mb = this.GetComponent<mainBehaviour>();
        BulletBehavior bb = bullet.GetComponent<BulletBehavior>();
        ModeChecker(bb);
        AfterShot(mb, bb);
        #region 没
        // 発射する速度を設定（ここでは10としていますが、必要に応じて調整してください）
        //float bulletSpeed = bb.bulletSpeed;
        //rb.velocity = transform.up * bb.bulletSpeed; // オブジェクトの向き(transform.up)に速度を与えます
        #endregion
    }

    protected void AfterShot(mainBehaviour mb, BulletBehavior bb)
    {
        mb.energy -= bb.energy * (int)coolDownRatio;
        coolDownTime[bulletId] = 0;
    }

    protected void ModeChecker(BulletBehavior bb)
    {
        if (mode == 0)
        {
            bb.speed = bb.maxSpeed;
            coolDownRatio = 1.0f;
            bb.isMode0 = true;
            bb.isMode1 = false;
            bb.isMode2 = false;
        }
        else if (mode == 1)
        {
            coolDownRatio = 1.0f;
            bb.isMode0 = false;
            bb.isMode1 = true;
            bb.isMode2 = false;
        }
        else if (mode == 2)
        {
            coolDownRatio = 1.0f;
            bb.isMode0 = false;
            bb.isMode1 = false;
            bb.isMode2 = true;
        }
        else if (mode == 3)
        {
            coolDownRatio = 1.5f;
            bb.isMode0 = false;
            bb.isMode1 = false;
            bb.isMode2 = false;
        }
    }

    // Fire() when there is a trajectory
    protected void Fire(List<Vector2> vectors)
    {
        if (!CanShoot()) return;
        Quaternion bulletRotation = transform.rotation;
        GameObject bullet = Instantiate(bulletPrefab[bulletId], this.transform.position, bulletRotation);
        BulletBehavior bb = bullet.GetComponent<BulletBehavior>();
        mainBehaviour mb = this.GetComponent<mainBehaviour>();
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        ModeChecker(bb);
        bb.SetTrajectory(vectors);
        bb.isTrajected = true;
        AfterShot(mb, bb);
        //bb.isTrajected = false;
    }
    protected void PrintMode()
    {
        Debug.Log("Current mode is:" + this.mode);
    }

    protected bool CanShoot()
    {
        mainBehaviour mb = this.GetComponent<mainBehaviour>();
        BulletBehavior bb = bulletPrefab[bulletId].GetComponent<BulletBehavior>();
        bool canShoot = (coolDownTime[bulletId] > bb.coolDown) && (mb.energy >= bb.energy * (int)coolDownRatio) && (!PauseManager.isPaused);
        return canShoot;
    }
}
