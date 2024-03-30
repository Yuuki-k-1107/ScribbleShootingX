using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    // スコアアタックモード
    public void StartScoreAttack()
    {
        CPUBehaviour.isVsCPU = false;
        ScoreManager.ResetScore();
        SceneManager.LoadScene("MainScene");
    }

    // CPUモード
    public void StartVSCPU()
    {
        CPUBehaviour.isVsCPU = true;
        SceneManager.LoadScene("MainScene");
    }

    // 軌道エディット
    public void Edit()
    {
        SceneManager.LoadScene("trajectory");
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    // ゲーム終了
    public void QuitGame()
    {
        Application.Quit();
    }
}
