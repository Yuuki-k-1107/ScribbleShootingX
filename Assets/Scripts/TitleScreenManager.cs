using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "TitleScene")
        GameObject.Find("InstructionCanvas").GetComponent<Canvas>().enabled = false;
    }

    // スコアアタックモード
    public void StartScoreAttack()
    {
        CPUBehaviour.isVsCPU = false;
        PauseFunctions.Resume();
        ScoreManager.ResetScore();
        SceneManager.LoadScene("MainScene");
    }

    // CPUモード
    public void StartVSCPU()
    {
        CPUBehaviour.isVsCPU = true;
        PauseFunctions.Resume();
        SceneManager.LoadScene("MainScene");
    }

    // 軌道エディット
    public void Edit()
    {
        SceneManager.LoadScene("trajectory");
    }

    // タイトルに戻る
    public static void BackToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    // ヘルプ表示
    public void ShowHelp()
    {
        GameObject.Find("InstructionCanvas").GetComponent<Canvas>().enabled = true;
    }

    // ゲーム終了
    public void QuitGame()
    {
        Application.Quit();
    }
}
