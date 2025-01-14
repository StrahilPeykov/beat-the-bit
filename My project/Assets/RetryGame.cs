using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryGame : MonoBehaviour
{
    public void RetryGameNow()
    {
        Debug.Log("Retrying the game...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }
}
