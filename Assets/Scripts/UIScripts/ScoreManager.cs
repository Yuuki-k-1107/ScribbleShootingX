using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static int score = 0;
    private TMP_Text scoreText;
    public LevelManager levelManager;
    // Start is called before the first frame update
    void Start()
    {
        if (!CPUBehaviour.isVsCPU)
        {
            scoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
            scoreText.enabled = !CPUBehaviour.isVsCPU;
        }
    }

    public static void ChangeScore(int amount)
    {
        score += amount;
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreText != null) UpdateText();
    }

    private void UpdateText()
    {
        //scoreText.text = $"SCORE\n{score}";
        scoreText.text = $"{score}";
    }

    public static void ResetScore()
    {
        score = 0;
    }
}
