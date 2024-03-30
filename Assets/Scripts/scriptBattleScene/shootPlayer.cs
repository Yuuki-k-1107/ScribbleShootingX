using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor.ShaderGraph.Internal;
#endif
using UnityEngine;
using UnityEngine.VFX;

public class shootPlayer : ShootingScript
{
    public override void Start()
    {
        myName = this.name;
        this.coolDownTime = new float[]{0, 0, 0, 0};
        dm = GameObject.Find("Empty").GetComponent<dataManager>();
        data = dm.Load($"{Application.dataPath}/data.json");
        print(dm);
        print(data);
    }

    public override void Update()
    {
        // 必殺技
        if(!PauseManager.isPaused) flashCount++;
        if (Input.GetKeyDown(KeyCode.F))
        {
            Flash();
        }
        // 弾の切り替え
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
            ShiftMode();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            List<Vector2> trajectoryData = data.bulletSaveData["0"].trajectoryData;
            if (trajectoryData != null)
            {
                Temporary(trajectoryData);
            }
            // trajectoryData = null
            else {
                print("No trajectory data");
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            List<Vector2> trajectoryData = data.bulletSaveData["0"].trajectoryData;
            if (trajectoryData != null)
            {
                float coef = 10f;
                int division = 7;
                Vector2 startPosition = Camera.main.ScreenToWorldPoint(this.transform.position);
                Vector2 initial = new Vector2(0f, 0f);
                Vector2 tempPoint = new Vector2(0f, 0f);
                int idx = 0;
                foreach (Vector2 point in trajectoryData)
                {
                    #region �v��
                    /*if (idx == 0)
                        // �����ŏ��̍��W�Ȃ�
                    {
                        initial = point;
                        tempPoint = point;
                        temp.Add(startPosition / coef);
                    }
                    else
                    {
                        // �O�̓_�ƌ��݂̓_�̒��_��ǉ�
                        temp.Add((((point + tempPoint) / 2) + startPosition - initial) / coef);

                        temp.Add((point + startPosition - initial) / coef);
                        // tempPoint ���X�V
                        tempPoint = point;
                    }
                    print($"index : {idx}");
                    idx++;*/
                    #endregion
                    #region �̗p��
                    if (idx == 0)
                    // �����ŏ��̍��W�Ȃ�
                    {
                        initial = point;
                        tempPoint = point;
                        temp.Add(new Vector2(0f, 0f));
                    }
                    else
                    {
                        // division��1���傫����
                        if (division > 1)
                        {
                            // i:(division-i)�ɓ�������_�ɑΉ�����x�N�g����ǉ�����
                            for (int i = 1; i < division; i++)
                            {
                                float newx = (i * point.y + (division - i) * tempPoint.y) / (division * coef);
                                float newy = (i * point.x + (division - i) * tempPoint.x) / (division * coef);
                                temp.Add(new Vector2(newx, newy));
                            }
                        }
                        #region division=2
                        // �O�̓_�ƌ��݂̓_�̒��_��ǉ�
                        /*                        float newx = (point.y + tempPoint.y) / (2 * coef);
                                                float newy = (point.x + tempPoint.x) / (2 * coef);
                                                temp.Add(new Vector2(newx,newy));*/
                        #endregion
                        temp.Add(new Vector2(point.y / coef, point.x / coef));
                        // tempPoint ���X�V
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
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z))
        {
            if (!myName.Equals("CPU"))
            {
               Fire();
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            temp.Add((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - startPoint);
        }
        if (Input.GetMouseButtonUp(0))
        {
            trajectory = temp.Select(item => item).ToList();
            temp.Clear();
            Fire(trajectory);
        }
        for (int i = 0; i < BULLET_NUM; i++)
        {
            coolDownTime[i] += Time.deltaTime * coolDownRatio * 2.0f;
        }
    }

    private void ShiftMode()
    {
        if (PauseManager.isPaused) return;
        mode += 1;
        mode %= MODE_NUM;
        Debug.Log($"Mode changed to {mode}");
    }

    protected override void Flash()
    {
        //print($"Flash() left:{flash}, flashCount:{flashCount}");
        if (PauseManager.isPaused) return;
        if (flash == 0) return;
        if (flashCount < 1000) return;
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
}
