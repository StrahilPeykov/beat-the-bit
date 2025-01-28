using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadLCM : MonoBehaviour
{
    public Button loadButton;

    void Start()
    {
        // Add a listener to the button to call LoadLevel when clicked
        loadButton.onClick.AddListener(loadLCM);
    }

    void loadLCM()
    {
        // Load the scene called "LevelSelection"
        SceneManager.LoadScene("LogicCircuitMaker");
    }
}
