using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedManager : MonoBehaviour
{
    // プレーヤーオブジェクト
    private GameObject player;
    // 弾情報のユーザインタフェース
    private GameObject[] uis = new GameObject[4];
    void Start()
    {
        // UI1-4を取得
        for (int i=0; i<4; i++)
        {
            uis[i] = GameObject.Find($"UI{i+1}");
        }
        player = GameObject.Find("Triangle");
        this.transform.position = uis[0].transform.position;
        //this.transform.position = new Vector3(125f, 690f, 0f);
    }

    void Update()
    {
        this.transform.position = uis[player.GetComponent<ShootingScript>().bulletId].transform.position;
        //this.transform.position = new Vector3(125f, 690f - player.GetComponent<ShootingScript>().bulletId * 180f,0f);
    }
}
