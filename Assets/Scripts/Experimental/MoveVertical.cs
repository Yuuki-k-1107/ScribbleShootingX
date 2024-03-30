using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVertical : MonoBehaviour
{
    public float amp = 1.0f;
    [SerializeField]
    private bool isUp = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(0f, isUp? (amp*Time.deltaTime) : (-amp * Time.deltaTime), 0f);
        this.transform.Translate(move);
    }
}
