using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelSelection : MonoBehaviour
{
    // Load a specific level by number (expects scenes named "Level1", "Level2", etc.)
    public void LoadLevel(int level)
    {
        SceneManager.LoadScene("Level" + level);
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("Level Selection");
    }

    // Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
