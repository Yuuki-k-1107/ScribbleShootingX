using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletKindAndAttribute : MonoBehaviour
{
    public int kind;
    public int attribute; 
    void Start()
    {
        kind = bulletKind.normal;
        attribute = bulletAttribute.normal;
    }

    void Update()
    {
        
    }
}
