using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBehaviour : BulletBehavior
{
    void Start()
    {
        Initialize(Attribute.Rock);
        SetParam(5f,10,0,999,6f,12f,3f,0f);
    }

    // Update is called once per frame
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

    protected override void DestroyObject()
    {
        this.GetComponent<Rigidbody2D>().isKinematic = true;
        this.GetComponent<Rigidbody2D>().freezeRotation = true;
        SetStarted();
        delayedDestroy = true;
    }
}
