using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteScript : MonoBehaviour
{
    public void OnLevelComplete(int stars)
    {

        Debug.Log("aha");
        if (LevelSelection.currLevel == LevelSelection.unlockedLevel)
        {
            LevelSelection.unlockedLevel++;
            PlayerPrefs.SetInt("UnlockedLevels", LevelSelection.unlockedLevel);
        }

        if (stars > PlayerPrefs.GetInt("stars" + LevelSelection.currLevel.ToString(), 0))
        {
            PlayerPrefs.SetInt("stars" + LevelSelection.currLevel.ToString(), stars);
        }

        Debug.Log("gata!");
        SceneManager.LoadScene("LevelSelection");
    }

}