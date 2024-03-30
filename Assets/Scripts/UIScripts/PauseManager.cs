using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (Input.GetKeyDown(KeyCode.P)/* || Input.GetKeyDown(KeyCode.Space)*/)
        {
            if (isPaused)
            {
                isPaused = false;
                Time.timeScale = 1;
            }
            else
            {
                isPaused = true;
                Time.timeScale = 0;
            }
        }
        pauseCanvas.enabled = isPaused;
    }
}
