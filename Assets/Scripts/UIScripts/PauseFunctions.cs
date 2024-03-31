using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseFunctions : MonoBehaviour
{
    private void Start()
    {
        
    }
    public static void Resume()
    {
        PauseManager.isPaused = false;
        Time.timeScale = 1;
    }

    public static void Pause()
    {
        PauseManager.isPaused = true;
        Time.timeScale = 0;
    }
}
