using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start the game by loading the Level Selection screen
    public void PlayButton()
    {
        SceneManager.LoadScene("LevelSelection");
    }

    // Open the Settings Menu
    public void SettingsButton()
    {
        SettingsMenu.Instance.Open();
    }

    // Quit the game (works in builds, not in the editor)
    public void QuitGame()
    {
        Application.Quit();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
