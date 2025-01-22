using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadLevelButton : MonoBehaviour
{
    public Button loadButton;

    void Start()
    {
        // Add a listener to the button to call LoadLevel when clicked
        loadButton.onClick.AddListener(LoadLevel);
    }

    void LoadLevel()
    {
        // Load the scene called "LevelSelection"
        SceneManager.LoadScene("LevelSelection");
    }
}
