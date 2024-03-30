using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public Canvas gameOverCanvas;
    public GameObject player;
    public GameObject CPU;
    private mainBehaviour mb;
    private int playerHP = 100;
    private bool isCPUExist = false;
    private bool isEnd = false;
    // Start is called before the first frame update
    void Start()
    {
        gameOverCanvas = GameObject.Find("GameOverCanvas").GetComponent<Canvas>();
        gameOverCanvas.enabled = false;
        player = GameObject.Find("Triangle");
        CPU = GameObject.Find("CPU");
        mb = player.GetComponent<mainBehaviour>();
        playerHP = mb.myHP;
        Time.timeScale = 1f;
        isCPUExist = CPU.IsUnityNull() ? false : true;
    }

    // Update is called once per frame
    void Update()
    {
        playerHP = mb.myHP;
        if(playerHP <= 0)
        {
            GameOver();
        }
        if (CPUBehaviour.isVsCPU && isCPUExist)
        {
            if(CPU.GetComponent<CPUBehaviour>().cpuHP <= 0)
            {
                isCPUExist = false;
                isEnd = true;
                GameOver();
            }
        }
        if (isEnd)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        gameOverCanvas.enabled = true;
        Time.timeScale = 0f;
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0))
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}
