using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBehaviour : BulletBehavior
{
    void Start()
    {
        Initialize(Attribute.Ice);
        SetParam(5f,10,5,0,6f,12f,2f,1f);
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        ColliderRule(collider2D);
    }

    void Update()
    {
        // ---------------------------
        if (DelayedDestroy()) return;
        Shrink(isMode0);
        Accelerate();
        // ---------------------------
        if (isDrawn)
        {
            PresetTrajectorialMove();
        }
        else if (isTrajected)
        {
            DraggedTrajectorialMove();
        }
        else
        {
            NormalMove();
        }
    }

}
