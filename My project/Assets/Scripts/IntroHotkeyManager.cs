using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroHotkeyManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
     if (Input.GetKeyDown(KeyCode.Return)) {
            SceneManager.LoadScene("MainMenu");
        }   
    }
}
