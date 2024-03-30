using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    // 親オブジェクト
    public GameObject parentObject = null;
    [SerializeField] string target = "Triangle";
    private mainBehaviour mb = null;
    private EnemyBehavior eb = null;
    private CPUBehaviour cb = null;
    private Image hpBar = null;
    // 体力値
    private int currentHP = 0;
    private int maxHP = 1;
    private float pinchThreshold = 0.3f;
    // 色
    private Color normalColor = new Color(0.2f, 0.8f, 0.2f, 1.0f);
    private Color pinchedColor = new Color(0.8f, 0.2f, 0.2f, 1.0f);
    // Start is called before the first frame update
    void Start()
    {
        parentObject = transform.parent.parent.gameObject;
        if(target.Equals("Triangle")) mb = parentObject.GetComponent<mainBehaviour>();
        if (target.Equals("Enemy")) eb = parentObject.GetComponent<EnemyBehavior>();
        if(target.Equals("CPU")) cb = parentObject.GetComponent <CPUBehaviour>();
        hpBar = this.GetComponent<Image>();
        if (mb != null)
        {
            this.maxHP = mb.myHP;
            this.currentHP = mb.myHP;
        }
        if (eb != null)
        {
            this.maxHP = eb.HP;
            this.currentHP = eb.HP;
        }
        if(cb != null)
        {
            this.maxHP = cb.cpuHP;
            this.currentHP = cb.cpuHP;
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = parentObject.transform.position - new Vector3(0f,0.1f,0f);
        if (mb != null) this.currentHP = mb.myHP;
        if (eb != null) this.currentHP = eb.HP;
        if(cb != null) this.currentHP = cb.cpuHP;
        BarUpdate();
    }

    private void BarUpdate()
    {
        float fillAmount = currentHP * 1.0f / maxHP;
        // print(fillAmount);
        if(fillAmount <= pinchThreshold)
        {
            hpBar.color = pinchedColor;
        } else
        {
            hpBar.color = normalColor;
        }
        hpBar.fillAmount = fillAmount;
    }
}
