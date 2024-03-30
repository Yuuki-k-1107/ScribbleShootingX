using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundBehavior : MonoBehaviour
{
    private GameObject ship;
    private Vector3 oldPos = new Vector3(0,0,0);
    public float scale = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        ship = GameObject.Find("Triangle");
        oldPos = ship.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = ship.transform.position;
        Vector3 delta = (newPos - oldPos) * scale;
        this.transform.Translate(delta);
        oldPos = newPos;
    }
}
