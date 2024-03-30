using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeUIManager : MonoBehaviour
{
    public ShootingScript shootingScript;
    // Manually sync with ShootingScript's one
    public const int MODE_NUM = 4;
    public Sprite[] sprites = new Sprite[MODE_NUM];
    public Image image;

    private int mode;
    void Start()
    {
        shootingScript = GameObject.Find("Triangle").GetComponent<ShootingScript>();
        image = this.GetComponent<Image>();
    }

    void Update()
    {
        GetMode();
        SetIcon();
    }

    private void GetMode()
    {
        this.mode = shootingScript.mode;
    }

    private void SetIcon()
    {
        image.sprite = sprites[mode];
    }
}
