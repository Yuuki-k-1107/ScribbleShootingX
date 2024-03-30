using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombGuide : MonoBehaviour
{
    [SerializeField]
    private float originalScale = 2f;
    private BombBehaviour bombBehaviour;
    // Start is called before the first frame update
    void Start()
    {
        bombBehaviour = GetComponentInParent<BombBehaviour>();
        if(bombBehaviour != null)
        {
            originalScale = bombBehaviour.explosionRange * 1.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localScale = new Vector3(
            originalScale / bombBehaviour.gameObject.transform.localScale.x,
            originalScale / bombBehaviour.gameObject.transform.localScale.y,
            1f);
        // Debug.Log(this.transform.lossyScale);
    }
}
