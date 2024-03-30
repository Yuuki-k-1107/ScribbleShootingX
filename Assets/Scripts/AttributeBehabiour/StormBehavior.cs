using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormBehavior : BulletBehavior
{
    [SerializeField]
    private string playerName;
    private PointEffector2D pe2d;
    void Start()
    {
        playerName = "Triangle";
        pe2d = GetComponent<PointEffector2D>();
        Initialize(Attribute.Storm);
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
        Inhale();
    }

    private void Inhale()
    {
        float radius = 6.0f;
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(this.transform.position, radius);
        foreach(Collider2D collider in collider2Ds)
        {
            GameObject obj = collider.gameObject;
            if (!(obj.Equals(this.gameObject)) && !obj.name.Equals(playerName))
            {
                Vector2 move = (obj.transform.position - this.transform.position);
                obj.transform.Translate(move * -0.0004f);
            }
        }
    }

}
