using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadMainMenu : MonoBehaviour
{
    public Button loadButton;

    void Start()
    {
        // Add a listener to the button to call LoadLevel when clicked
        loadButton.onClick.AddListener(loadMainMenu);
    }

    void loadMainMenu()
    {
        // Load the scene called "LevelSelection"
        SceneManager.LoadScene("MainMenu");
    }
}
