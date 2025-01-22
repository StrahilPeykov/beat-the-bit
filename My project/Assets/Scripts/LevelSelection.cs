using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelSelection : MonoBehaviour
{
    public LevelObject[] levelObjects;
    public static int currLevel;
    public static int unlockedLevel;
    public Sprite Goldenstar;

    // Load a specific level by number (expects scenes named "Level1", "Level2", etc.)
    public void LoadLevel(int level)
    {
        if (level == 0)
        {
            currLevel = 0;
            SceneManager.LoadScene("GraphGame");
        }
        else if (level == 1 && unlockedLevel >= 1)
        {
            currLevel = 1;
            SceneManager.LoadScene("Graph2");
        }
        else if (level == 2 && unlockedLevel >= 2)
        {
            currLevel = 2;
            SceneManager.LoadScene("LogicGates");
        } else if (level == 3 && unlockedLevel >= 3)
        {
            currLevel = 3;
            SceneManager.LoadScene("CryptographyGame");
        }
    }

    void Start()
    {
        unlockedLevel = PlayerPrefs.GetInt("UnlockedLevels", 0);

        Debug.Log(unlockedLevel);


        for (int i = 0; i < levelObjects.Length; i++)
        {
            if (unlockedLevel >= i)
            {
                if (levelObjects[i].levelButton != null &&
                            levelObjects[i].levelButton.GetComponent<ButtonHoverEffect>() != null)
                {
                    levelObjects[i].levelButton.GetComponent<ButtonHoverEffect>().isUnlocked = true;
                    int stars = PlayerPrefs.GetInt("stars" + i.ToString(), 0);
                    Debug.Log(stars);
                    for (int j = 0; j < stars; j++)
                    {
                        
                        levelObjects[i].stars[j].sprite = Goldenstar;
                    }
                }
                else
                {
                    Debug.LogError($"Null reference at index {i} for levelButton or ButtonHoverEffect component.");
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }
    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
