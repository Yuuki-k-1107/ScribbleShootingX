using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlBehaviour : MonoBehaviour
{
    //速度用の変数
    [SerializeField] float speed;
    //移動判定用の変数
    bool isMoving;
    Vector3 mousePos, worldPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y,10f));
        transform.position = Vector3.MoveTowards(transform.position, worldPos, speed * Time.deltaTime);
    }
}
