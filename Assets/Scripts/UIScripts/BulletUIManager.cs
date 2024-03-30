using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletUIManager : MonoBehaviour
{
    private Image image;
    private string objectName;
    private GameObject bulletPrefab = null;
    private GameObject player = null;
    private ShootingScript ss = null;
    private mainBehaviour mb = null;
    private Color bulletColor;
    private Color darkColor;
    private float energyConsumption;
    private float coolDown = 0f;
    private int id = 0;
    // Start is called before the first frame update
    void Start()
    {
        image = this.GetComponent<Image>();
        player = GameObject.Find("Triangle");
        objectName = gameObject.name;
        if(player != null)
        {
            ss = player.GetComponent<ShootingScript>();
            mb = player.GetComponent <mainBehaviour>();
        }
        switch (objectName)
        {
            case "UI1":
                SetBulletPrefab(0);
                break;

            case "UI2":
                SetBulletPrefab(1);
                break;

            case "UI3":
                SetBulletPrefab(2);
                break;

            case "UI4":
                SetBulletPrefab(3);
                break;

            default:
                break;
        }
        if (bulletPrefab != null)
        {
            BulletBehavior bb = bulletPrefab.GetComponent<BulletBehavior>();
            this.coolDown = bb.coolDown;
            image.color = bulletColor;
            energyConsumption = bb.energy;
            darkColor = new Color(
                bulletColor.r * 0.5f,
                bulletColor.g * 0.5f,
                bulletColor.b * 0.5f,
                bulletColor.a
                );
            bb.enabled = false;
        }
    }

    private void SetBulletPrefab(int id)
    {
        bulletPrefab = Instantiate(ss.bulletPrefab[id], transform.position, transform.rotation);
        this.id = id;
        bulletColor = bulletPrefab.GetComponent<SpriteRenderer>().color;
        print(bulletColor);
        bulletPrefab.transform.localScale = new Vector3(100f, 100f, 1f);
        bulletPrefab.transform.parent = this.transform;
        bulletPrefab.layer = gameObject.layer;
        // Disable bombprefab's guide
        if (bulletPrefab.name.Contains("BombBullet"))
        {
            bulletPrefab.transform.Find("Guide").gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float fillAmount = Mathf.Min((ss.coolDownTime[id] / coolDown), 1.0f);
        image.fillAmount = fillAmount;
        if(fillAmount < 1f || energyConsumption > mb.energy)
        {
            image.color = darkColor;
        } else
        {
            image.color = bulletColor;
        }
    }
}
