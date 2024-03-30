using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class saveButtonClick : MonoBehaviour
{
    GameObject objCanvas;
    dataManager manager;
    drawCanvas canvas;
    bulletKindAndAttribute ka;
    Button button;
    public saveData data;
    public Dictionary<string, bulletData> bullet;

    public int saveNumber;

    private void Start()
    {
        objCanvas = GameObject.Find("Cube");
        canvas = objCanvas.gameObject.GetComponent<drawCanvas>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        manager = GameObject.Find("manager").GetComponent<dataManager>();
        data = manager.data;
        bullet = data.bulletSaveData;
        ka = objCanvas.gameObject.GetComponent<bulletKindAndAttribute>();
    }

    private void OnClick()
    {
        if(canvasFlag.isSave){
            if(canvas.lineCost > 0){
                string s = saveNumber.ToString();
                bulletData bData = new bulletData();
                bData.costData = canvas.lineCost;
                bData.trajectoryData = new List<Vector2>(canvas.tList);
                bData.startPositionData = canvas.startPosition;
                bData.kindData = ka.kind;
                bData.attributeData = ka.attribute;

                if (!bullet.ContainsKey(s))
                {
                    bullet.Add(s, bData);
                }
                else
                {
                    bullet[s] = bData;
                }
                manager.Save(data);
            }
        }
        else{
            string s = saveNumber.ToString();
            if(bullet.ContainsKey(s)){
                bulletData bData = bullet[saveNumber.ToString()];
                float costData = bData.costData;
                Vector2 startPositionData = bData.startPositionData;
                List<Vector2> tList = new List<Vector2>(bData.trajectoryData);
                ka.kind = bData.kindData;
                ka.attribute = bData.attributeData;
                canvas.loadData(tList, costData, startPositionData);
            }
        }
    }
}
