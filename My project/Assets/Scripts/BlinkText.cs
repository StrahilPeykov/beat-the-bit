using System.Collections;
using TMPro;
using UnityEngine;

public class BlinkText : MonoBehaviour
{
    public TextMeshProUGUI textToBlink; // Reference to the TextMeshPro text component
    public float blinkInterval = 0.5f;  // Interval in seconds for blinking

    private bool isBlinking = true;

    void Start()
    {
        if (textToBlink == null)
        {
            textToBlink = GetComponent<TextMeshProUGUI>();
        }

        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while (isBlinking)
        {
            textToBlink.enabled = !textToBlink.enabled; // Toggle text visibility
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    public void StopBlinking()
    {
        isBlinking = false; // Stop blinking if needed
        textToBlink.enabled = true; // Ensure the text is visible
    }
}