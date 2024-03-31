using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OnClickCloseCanvas : MonoBehaviour
{
    public Canvas canvas;
    void Start()
    {
        canvas = this.GetComponent<Canvas>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            canvas.enabled = false;
        }
    }
}
