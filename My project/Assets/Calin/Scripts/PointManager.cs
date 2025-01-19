// using UnityEngine;
// using TMPro;
// using System;
// using System.Collections.Generic;
// public class PointManager : MonoBehaviour
// {
//     public TextUpdater textUpdater;

//     public static int points; // Tracks the current points

//     void Start()
//     {
//         points = 0; // Initialize points to 0
//         UpdatePointDisplay(); // Update the display at the start
//     }

//     // Public static method to add points  
//     public static void AddPoints(int amount)
//     {
//         points += amount; // Add points
//         UpdatePointDisplay(); // Update the binary display
//     }

//     // Public static method to subtract points
//     public static void SubtractPoints(int amount)
//     {
//         points = Mathf.Max(0, points - amount); // Subtract points, ensuring it doesn't go below 0
//         // UpdatePointDisplay(); // Update the binary display

//         textUpdater.scoreText.text = ConvertToBinary(points);

//     }

//     // Updates the point display in binary
//     public static void UpdatePointDisplay()
//     {
//         // if (pointText != null) // Check if pointText is assigned
//         // {
//         //     string binaryPoints = ConvertToBinary(points); // Convert points to binary
//         //     pointText.text = binaryPoints; // Update the UI with binary representation
//         //     Debug.Log(binaryPoints);
//         // }

//         textUpdater.scoreText.text = ConvertToBinary(points);

//     }

//     // Helper static method to convert an integer to binary
//     public static string ConvertToBinary(int number)
//     {
//         string binary = System.Convert.ToString(number, 2); // Convert to binary
//         return binary.PadLeft(6, '0'); // Ensure the binary string is at least 6 characters long
//     }
// }
