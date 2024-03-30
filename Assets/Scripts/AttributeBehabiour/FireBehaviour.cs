using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBehaviour : BulletBehavior
{
    void Start()
    {
        Initialize(Attribute.Fire);
        SetParam(3f, 12, 7, -1, 6f, 12f, 1f, 3f);
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
