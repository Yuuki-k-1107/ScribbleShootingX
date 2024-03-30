using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonArranger : MonoBehaviour
{
    private const int BUTTON_NUM = 4;
    [SerializeField]
    private Button[] buttons = new Button[BUTTON_NUM];
    private RectTransform[] rectTransforms = new RectTransform[BUTTON_NUM];
    
    void Start()
    {
        GetRectTransforms();
        ArrangeButton();
    }

    private void GetRectTransforms()
    {
        for (int i = 0; i < BUTTON_NUM; i++)
        {
            rectTransforms[i] = buttons[i].GetComponent<RectTransform>();
        }
    }

    private void ArrangeButton()
    {
        for(int i = 0; i < 4; i++)
        {
            rectTransforms[i].position = new Vector3(80+(i%2)*(-160), (i/2)*(-80), 0);
        }
    }
}
