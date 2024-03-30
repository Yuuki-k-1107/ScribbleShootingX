using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loadButtonTrue : MonoBehaviour
{
    Button button;
    
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        canvasFlag.isSave = false;
        canvasFlag.isSaveOrLoad = true;
    }
}