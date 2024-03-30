using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBehaviour : BulletBehavior
{
    void Start()
    {
        Initialize(Attribute.Cannon);
        SetParam(10f,100,100,2,1f,5f,5f,0.5f);
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
