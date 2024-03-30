using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlashUIManager : MonoBehaviour
{
    public Image bar;
    public TMP_Text timesLeftText;
    public TMP_Text pressFText;
    public float blinkRate = 1.0f;

    private float nextBlinkTime;
    private bool isPressFVisible = false;
    private ShootingScript script;
    private Color activeColor = new Color(0.8f, 0.1f, 0.1f, 1f);
    private Color notActiveColor = new Color(0.5f, 0.5f, 0.5f, 1f);
    void Start()
    {
        script = GameObject.Find("Triangle").GetComponent<ShootingScript>();
        timesLeftText = this.GetComponentInChildren<TMP_Text>();
        bar = this.transform.Find("RefreshBar").gameObject.GetComponent<Image>();
        pressFText = this.transform.Find("PressFText").gameObject.GetComponent<TMP_Text>();
        nextBlinkTime = Time.time + blinkRate;
    }

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        float fillAmount = Mathf.Min(script.flashCount * 1.0f /1000, 1f);
        bar.color = (fillAmount == 1.0f) ? activeColor : notActiveColor;
        if (fillAmount < 1.0f) isPressFVisible = false;
        if (script.flash > 0)
        {
            bar.fillAmount = fillAmount;
        } else
        {
            bar.fillAmount = 0f;
            bar.color = notActiveColor;
        }
        timesLeftText.text = $"{script.flash}";
        BlinkText(fillAmount);
    }

    void BlinkText(float fillAmount)
    {
        if (Time.time >= nextBlinkTime)
        {
            nextBlinkTime = Time.time + blinkRate;
            if (fillAmount == 1.0f)
            {
                isPressFVisible = !isPressFVisible;
            }
            else
            {
                isPressFVisible = false;
            }
        }
        pressFText.alpha = isPressFVisible ? 1.0f : 0.0f;
    }
}
