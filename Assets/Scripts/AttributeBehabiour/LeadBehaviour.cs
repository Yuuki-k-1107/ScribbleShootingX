using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeadBehaviour : BulletBehavior
{
    void Start()
    {
        Initialize(Attribute.Lead);
        SetParam(10f, 15, 10, 0, 6f, 12f, 3f, 0.1f);
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
