using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public Canvas pauseCanvas;
    public static bool isPaused = false;
    void Start()
    {
        pauseCanvas = this.GetComponent<Canvas>();
        pauseCanvas.enabled = isPaused;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isPaused)
            {
                PauseFunctions.Resume();
            }
            else
            {
                PauseFunctions.Pause();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                TitleScreenManager.BackToTitle();
            }
        }
        pauseCanvas.enabled = isPaused;
    }
}
