using UnityEngine;
using TMPro;

public class BinaryCodeFullScreen : MonoBehaviour
{
    public TextMeshProUGUI binaryText; // Assign TextMeshPro component here
    public int rows = 70;             // Number of rows of binary text
    public int columns = 200;          // Number of columns in each row
    public float updateInterval = 0.1f; // Refresh interval for binary

    private void Start()
    {
        StartCoroutine(UpdateBinaryText());
    }

    private System.Collections.IEnumerator UpdateBinaryText()
    {
        while (true)
        {
            binaryText.text = GenerateBinaryScreen(rows, columns);
            yield return new WaitForSeconds(updateInterval);
        }
    }

    private string GenerateBinaryScreen(int rows, int columns)
    {
        System.Text.StringBuilder binary = new System.Text.StringBuilder();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                binary.Append(Random.Range(0, 2)); // Append 0 or 1 randomly
            }
           // binary.Append("\n"); // Add a new line after each row
        }
        return binary.ToString();
    }
}
  