using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnergyController : MonoBehaviour
{
    public Image healthBar;  // �G�l���M�[�Q�[�W��Image
    public TMP_Text healthText;  // �G�l���M�[�e�L�X�g�iTextMes�G�l���M�[ro�j
    
    public int maxEnergy = 255;  // �ő�G�l���M�[
    private float currentEnergy;    // ���݂̃G�l���M�[
    private GameObject player;

    void Start()
    {
        this.player = GameObject.Find("Triangle"); // �v���C���[���������ɓ���
        maxEnergy = player.GetComponent<mainBehaviour>().energy;
        currentEnergy = maxEnergy;
        UpdateUI();
    }

    void Update()
    {
        currentEnergy = player.GetComponent<mainBehaviour>().energy;
        UpdateUI();
    }

    // UI���X�V���郁�\�b�h
    void UpdateUI()
    {
        // �G�l���M�[�Q�[�W���X�V
        float fillAmount = currentEnergy / maxEnergy;
        healthBar.fillAmount = fillAmount;

        // �G�l���M�[�e�L�X�g���X�V
        healthText.text = $"{currentEnergy}\n/{maxEnergy}";
    }
}
