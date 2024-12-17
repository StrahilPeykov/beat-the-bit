using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
        // Enforce singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object alive across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Reload the current level
    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Load the next level based on naming convention ("Level1", "Level2", etc.)
    public void LoadNextLevel()
    {
        int currentLevel = GetCurrentLevelNumber();
        string nextLevel = "Level" + (currentLevel + 1);
        
        // Check if the next level exists in the build settings
        if (Application.CanStreamedLevelBeLoaded(nextLevel))
        {
            SceneManager.LoadScene(nextLevel);
        }
        else
        {
            Debug.Log("No next level exists: returning to Level Selection.");
            SceneManager.LoadScene("LevelSelection");
        }
    }

    // Extract the current level number from the scene name
    private int GetCurrentLevelNumber()
    {
        int levelNumber;
        int.TryParse(SceneManager.GetActiveScene().name.Replace("Level", ""), out levelNumber);
        return levelNumber;
    }
}
