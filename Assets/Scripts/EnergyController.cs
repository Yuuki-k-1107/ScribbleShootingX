using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnergyController : MonoBehaviour
{
    public Image healthBar;  // エネルギーゲージのImage
    public TMP_Text healthText;  // エネルギーテキスト（TextMesエネルギーro）
    
    public int maxEnergy = 255;  // 最大エネルギー
    private float currentEnergy;    // 現在のエネルギー
    private GameObject player;

    void Start()
    {
        this.player = GameObject.Find("Triangle"); // プレイヤー名をここに入力
        maxEnergy = player.GetComponent<mainBehaviour>().energy;
        currentEnergy = maxEnergy;
        UpdateUI();
    }

    void Update()
    {
        currentEnergy = player.GetComponent<mainBehaviour>().energy;
        UpdateUI();
    }

    // UIを更新するメソッド
    void UpdateUI()
    {
        // エネルギーゲージを更新
        float fillAmount = currentEnergy / maxEnergy;
        healthBar.fillAmount = fillAmount;

        // エネルギーテキストを更新
        healthText.text = $"{currentEnergy}\n/{maxEnergy}";
    }
}
