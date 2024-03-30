using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    // �X�R�A�A�^�b�N���[�h
    public void StartScoreAttack()
    {
        CPUBehaviour.isVsCPU = false;
        ScoreManager.ResetScore();
        SceneManager.LoadScene("MainScene");
    }

    // CPU���[�h
    public void StartVSCPU()
    {
        CPUBehaviour.isVsCPU = true;
        SceneManager.LoadScene("MainScene");
    }

    // �O���G�f�B�b�g
    public void Edit()
    {
        SceneManager.LoadScene("trajectory");
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    // �Q�[���I��
    public void QuitGame()
    {
        Application.Quit();
    }
}
