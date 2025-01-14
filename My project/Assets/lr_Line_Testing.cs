using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class lr_Testing : MonoBehaviour
{
    [SerializeField] private Transform[] points; // Array to store the points for the line.
    [SerializeField] private lr_Line_Controller line; // Reference to the custom Line Controller script.

    private void Start()
    {
        line.SetUpLine(points); // Calls the SetUpLine method on the Line Controller to initialize the line.
    }
}

