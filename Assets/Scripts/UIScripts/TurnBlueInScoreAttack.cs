using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnBlueInScoreAttack : MonoBehaviour
{
    private GameObject scoreObject;
    void Start()
    {
        scoreObject = GameObject.Find("ScoreBack");
        if (!CPUBehaviour.isVsCPU)
        {
            // if playMode == ScoreAttack, then turn the right UI color to the left one.
            this.GetComponent<Image>().color = new Color(86 * 1.0f / 255, 103 * 1.0f / 255, 115 * 1.0f / 255, 1.0f);
        }
        else
        {
            scoreObject.SetActive(false);
        }
    }
}
