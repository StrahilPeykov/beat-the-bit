using UnityEngine;
using TMPro;

public class TextUpdater : MonoBehaviour
{
    public TextMeshProUGUI formulaText; // Reference to the TextMeshPro component
    public TextMeshProUGUI scoreText; // Reference to the TextMeshPro component


    public static string formula, score;
    void Start()
    {
        // Example: Set initial text
        // if (textMeshPro != null)
        // {
        //     textMeshPro.text = "Hello, World!";
        // }
    }

    // Method to dynamically update the text
    public void UpdateScoreText(string newText)
    {
        if (scoreText != null)
        {
            scoreText.text = newText;
        }
    }
}
