using UnityEngine;

public class DialogueSound : MonoBehaviour
{
    public AudioSource audioSource; // The Audio Source component

    // Method to play the sound
    public void PlaySound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource is not assigned!");
        }
    }
    
}
