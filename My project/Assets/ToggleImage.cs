using UnityEngine;
using UnityEngine.UI;

public class ToggleImage : MonoBehaviour
{
    public GameObject imageToToggle; // Assign the Image GameObject in the Inspector

    // This function will be called by the Button
    public void toggle()
    {
        if (imageToToggle != null)
        {
            // Toggle the image's active state
            imageToToggle.SetActive(!imageToToggle.activeSelf);
        }
    }
}
