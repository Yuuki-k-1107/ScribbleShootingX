using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class saveButtonCreate : MonoBehaviour
{
    public RectTransform prefab;
    public RectTransform content;
    saveButtonClick script;

    public void Start(){
        int numberOfBulletData = saveData.numberOfBulletData;

        for(int i = 0; i < numberOfBulletData; i++){
            // Itemを生成
            var item = GameObject.Instantiate(prefab);
            // Contentの子として登録
            item.SetParent(content, false);
            // 保存するセーブデータのインデックスを指定
            script = item.gameObject.transform.Find("ButtonSaveWindow").gameObject.GetComponent<saveButtonClick>();
            script.saveNumber = i;
            item.gameObject.transform.Find("TextSaveWindow").gameObject.GetComponent<TextMeshProUGUI>().SetText((i+1).ToString());
        }
    }
}