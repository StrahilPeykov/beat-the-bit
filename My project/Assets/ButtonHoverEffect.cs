using TMPro; // Import TextMeshPro namespace
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI buttonText; // Reference to the button's TextMeshPro component
    public Color lockedColor = Color.red; // Default color for locked levels

    public TMP_InputField rootNodeInputField; // Reference to the Root Node input field
    public LineRenderer lineRenderer;
   

    public Color lockedHoverColor = Color.yellow; // Hover color for locked levels
    public Color unlockedDefaultColor = Color.white; // Default color for unlocked levels
    public Color unlockedHoverColor = Color.green; // Hover color for unlocked levels
    public bool isUnlocked; // Flag to check if the level is unlocked

    private Color originalTextColor;

    void Start()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            // Set the text color based on whether the level is unlocked
            originalTextColor = isUnlocked ? unlockedDefaultColor : lockedColor;
            buttonText.color = originalTextColor;
        }

        if (rootNodeInputField != null)
        {
            rootNodeInputField.text = "root node";
        }
    }

    void Update()
    {
        if (lineRenderer != null && isUnlocked)
        {
            Debug.Log("Updating LineRenderer color to green...");

            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.green, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
            );

            lineRenderer.colorGradient = gradient;
        }
        else
        {
            if (lineRenderer == null)
            {
                Debug.LogError("LineRenderer is not assigned!");
            }
            if (!isUnlocked)
            {
                Debug.Log("isUnlocked is false!");
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonText != null)
        {
            // Change to hover color based on locked or unlocked state
            buttonText.color = isUnlocked ? unlockedHoverColor : lockedHoverColor;
        }

        if (rootNodeInputField != null)
        {
            rootNodeInputField.text = isUnlocked ? "available" : "unavailable";
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonText != null)
        {
            // Revert to the original color
            buttonText.color = originalTextColor;
        }

        if (rootNodeInputField != null)
        {
            rootNodeInputField.text = "root node";
        }
    }
}