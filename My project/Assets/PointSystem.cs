using UnityEngine;
using TMPro;

public class PointSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointText; // Reference to the TextMeshProUGUI for displaying points
    private int points; // Tracks the current points

    void Start()
    {
        points = 0; // Initialize points to 0
        UpdatePointDisplay(); // Update the display at the start
    }

    // Public method to add points  
    public void AddPoints(int amount)
    {
        points += amount; // Add points
        UpdatePointDisplay(); // Update the binary display
    }

    // Public method to subtract points
    public void SubtractPoints(int amount)
    {
        points = Mathf.Max(0, points - amount); // Subtract points, ensuring it doesn't go below 0
        UpdatePointDisplay(); // Update the binary display
    }

    // Updates the point display in binary
    private void UpdatePointDisplay()
    {
        string binaryPoints = ConvertToBinary(points); // Convert points to binary
        pointText.text = binaryPoints; // Update the UI with binary representation
    }

    // Helper method to convert an integer to binary
    private string ConvertToBinary(int number)
    {
        string binary = System.Convert.ToString(number, 2); // Convert to binary
        return binary.PadLeft(6, '0'); // Ensure the binary string is at least 6 characters long
    }
}