using UnityEngine;
using System;

public class SaveScore {
    
    public static void UpdateScore()
    {
        
        float totalTime = (float)(GameInformation.end - GameInformation.start).TotalMinutes;
        if (GameInformation.level > PlayerPrefs.GetInt("LEVEL") || (GameInformation.level > PlayerPrefs.GetInt("LEVEL") && totalTime < PlayerPrefs.GetFloat("TIME")))
        {
            PlayerPrefs.SetInt("LEVEL", GameInformation.level);
            PlayerPrefs.SetFloat("TIME", totalTime);
        }
        
    }
}
