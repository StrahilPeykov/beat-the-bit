using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PopupWindow : MonoBehaviour
{
    public GameObject popupPanel; // The Panel GameObject
    public TextMeshProUGUI successText; // The TextMeshPro text component
    public float fadeDuration = 2f; // Time to fade in/out
    public float displayDuration = 2f; // Time to display the popup before it fades out

    private CanvasGroup canvasGroup;

    void Start()
    {
        // Get the CanvasGroup component (used for fading)
        canvasGroup = popupPanel.GetComponent<CanvasGroup>();

        // Initially hide the popup
        popupPanel.SetActive(false);
    }

    public void ShowPopup()
    {
        StartCoroutine(FadeInAndOut());
    }

    IEnumerator FadeInAndOut()
    {
        // Show the panel and set alpha to 0 (invisible)
        popupPanel.SetActive(true);
        canvasGroup.alpha = 0;

        // Fade in
        float timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1;

        // Wait for display duration
        yield return new WaitForSeconds(displayDuration);

        // Fade out
        timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0;

        // Hide the panel after fading out
        popupPanel.SetActive(false);
    }
}
