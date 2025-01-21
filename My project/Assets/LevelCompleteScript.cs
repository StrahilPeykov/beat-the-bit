using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteScript : MonoBehaviour
{

    private int stars;
    public void OnLevelComplete()
    {

        
        if (LevelSelection.currLevel == LevelSelection.unlockedLevel && stars == 3)

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


    public void changeStars(int number)
    {
        this.stars = number;
    }


}