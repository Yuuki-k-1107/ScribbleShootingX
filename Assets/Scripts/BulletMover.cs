using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMover : MonoBehaviour
{
    private BulletBehavior bulletBehavior;
    // Start is called before the first frame update
    void Start()
    {
        bulletBehavior = this.gameObject.GetComponent<BulletBehavior>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
