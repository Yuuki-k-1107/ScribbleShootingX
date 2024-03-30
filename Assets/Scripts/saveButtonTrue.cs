using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class saveButtonTrue : MonoBehaviour
{
    Button button;
    
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        canvasFlag.isSave = true;
        canvasFlag.isSaveOrLoad = true;
    }
}