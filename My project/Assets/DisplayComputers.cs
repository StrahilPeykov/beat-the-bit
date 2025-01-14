using UnityEngine;

public class DisplayComputers : MonoBehaviour
{
    public GameObject[] computers; // Array to store computer GameObjects

    // Method to activate computers
    public void ShowComputers()
    {
        foreach (GameObject computer in computers)
        {
            computer.SetActive(true); // Enable the GameObject
        }
    }
}
