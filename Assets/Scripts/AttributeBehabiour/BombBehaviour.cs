using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehaviour : BulletBehavior
{
    public float explosionRange = 2f;
    // Start is called before the first frame update
    void Start()
    {
        Initialize(Attribute.Bomb);
        SetParam(1f,30,10,0,3f,9f,5f,1f);
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

    protected override void DestroyObject()
    {
        Explode();
        base.DestroyObject();
    }

    private void Explode()
    {
        // 中心位置を現在のオブジェクトの位置とし、半径を2として円の範囲内にあるコライダーを取得
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRange);

        // 範囲内のすべてのコライダーに対して処理を行う
        foreach (Collider2D collider in colliders)
        {
            // 自分自身のコライダーを無視する
            if (collider.gameObject != this.gameObject)
            {
                EnemyBehavior eb = collider.gameObject.GetComponent<EnemyBehavior>();
                if (eb != null)
                {
                    // 敵にダメージ
                    eb.HP -= 100;
                }
            }
        }
    }
}
