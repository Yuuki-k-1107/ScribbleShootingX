using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private int[] thresholds = new int[] { 0, 1000, 3000, 5000, 10000, 20000, 40000, 60000};
    
    public int GetLevelFromScore(int score)
    {
        for(int i = 1; i < thresholds.Length; i++)
        {
            if(score < thresholds[i])
            {
                return i;
            }
        }
        return thresholds.Length;
    }
}
