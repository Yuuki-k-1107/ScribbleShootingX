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
        // ���S�ʒu�����݂̃I�u�W�F�N�g�̈ʒu�Ƃ��A���a��2�Ƃ��ĉ~�͈͓̔��ɂ���R���C�_�[���擾
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRange);

        // �͈͓��̂��ׂẴR���C�_�[�ɑ΂��ď������s��
        foreach (Collider2D collider in colliders)
        {
            // �������g�̃R���C�_�[�𖳎�����
            if (collider.gameObject != this.gameObject)
            {
                EnemyBehavior eb = collider.gameObject.GetComponent<EnemyBehavior>();
                if (eb != null)
                {
                    // �G�Ƀ_���[�W
                    eb.HP -= 100;
                }
            }
        }
    }
}
