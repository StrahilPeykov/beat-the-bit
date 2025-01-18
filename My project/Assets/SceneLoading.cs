using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class SceneLoading : MonoBehaviour
{

    public void LevelSelection()
    {
        Debug.Log("boii");
        StartCoroutine(LoadSceneWithDelay());
    }

    private IEnumerator LoadSceneWithDelay()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second
        SceneManager.LoadScene("LevelSelection"); // Replace with your actual scene name
    }


}
