using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Threading;


public class FormulaChecker : MonoBehaviour
{

    public TextMeshProUGUI formulaText; // Reference to the TextMeshPro component
    public TextMeshProUGUI scoreText; 
    
    public GameObject correctText; // Reference to the TextMeshPro component
    public GameObject incorrectText; // Reference to the TextMeshPro component

    // public PointManager pointManager;
    public Button myButton;
    string[] inputs2Vars = { "00", "01", "10", "11" };
    string[] inputs3Vars = { "000", "001", "010", "011", "100", "101", "110", "111" };
    string[] inputs4Vars = { "0000", "0001", "0010", "0011", "0100", "0101", "0110", "0111", "1000", "1001", "1010", "1011", "1100", "1101", "1110", "1111" };

    string[] letters = { "A", "B", "C", "D" };
    // public TextUpdater textUpdater;

    public static int points; // Tracks the current points

    string formulaCopy;



    // Public static method to add points  
    public void AddPoints(int amount)
    {
        points += amount; // Add points
        UpdatePointDisplay(); // Update the binary display
    }

    // Public static method to subtract points
    public void SubtractPoints(int amount)
    {
        points = Mathf.Max(0, points - amount); // Subtract points, ensuring it doesn't go below 0
        // UpdatePointDisplay(); // Update the binary display

        scoreText.text = ConvertToBinary(points);

    }

    // Updates the point display in binary
    public void UpdatePointDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = ConvertToBinary(points);
        }
        else
        {
            Debug.LogWarning("scoreText is not assigned!");
        }
    }

    public void UpdateFormulaDisplay()
    {
        if (formulaText != null)
        {
            formulaText.text = "To make: " + FormulaManager.getNewFormula();
        }
        else
        {
            Debug.LogWarning("scoreText is not assigned!");
        }
    }

    public void UpdateCheckFormulaDisplay(string checkText)
    {
        if (formulaText != null)
        {
            formulaText.text = checkText;
        }
        else
        {
            Debug.LogWarning("scoreText is not assigned!");
        }
    }


    // Helper static method to convert an integer to binary
    public static string ConvertToBinary(int number)
    {
        string binary = System.Convert.ToString(number, 2); // Convert to binary
        return binary.PadLeft(6, '0'); // Ensure the binary string is at least 6 characters long
    }
    void Start()
    {
        points = 0;

        if (scoreText == null)
        {
            Debug.Log("scoreText is not assigned in the Inspector!");
        }

        if (myButton != null)
        {
            myButton.onClick.AddListener(OnClick);
        }
        else
        {
            Debug.Log("myButton is not assigned in the Inspector!");
        }

        UpdatePointDisplay();
    }


    void OnClick()
    {
        checkFormula();
    }

    void resetInputs()
    {
        foreach (Switch input in FormulaManager.inputList)
        {
            PauseAndSetValue(input, "0", .1f);
        }
    }

    void checkFormula()
    {
        formulaCopy = FormulaManager.formula.Item1;
        // this.AddPoints(10);
        if (FormulaManager.inputList.Count != FormulaManager.formula.Item2 || FormulaManager.probeCnt == 0)
        {
            Debug.Log("Incorrect!");
        }
        else
        {
            // Call the coroutine
            StartCoroutine(CheckFormulaWithDelay());
        }
    }

    IEnumerator CheckFormulaWithDelay() // Note: `IEnumerator` (non-generic)
    {
        foreach (var kvp in FormulaManager.formula.Item3)
        {
            // resetInputs();
            int i = 0;
            string key = kvp.Key;

            string checkText = "Test Case: ";




            foreach (Switch input in FormulaManager.inputList)
            {
                Debug.Log(input.varName);
                // Set each value with a delay
                yield return StartCoroutine(PauseAndSetValue(input, key.Substring(i, 1), .2f)); // 1.5 seconds delay
                // checkText += input.varName + " = " + key.Substring(i, 1);
                i++;
            }
            i = 0;

            foreach (char c in key)
            {
                checkText += $"{letters[i]} = {c.ToString()} ";
                i++;
            }
            UpdateCheckFormulaDisplay(checkText);

            if (FormulaManager.probe.getSignal() != kvp.Value)
            {
                UpdateCheckFormulaDisplay("To make: " + formulaCopy + '\n' + checkText + "failed!");
                

                Debug.Log("Incorrect!");

                incorrectText.SetActive(true);

                // Thread.Sleep(2000);

                incorrectText.SetActive(false);

                yield break; // Exit the coroutine if the formula is incorrect
            }
        }
        // Debug.Log(pointManager.ConvertToBinary(10));
        this.AddPoints(10);
        UpdateCheckFormulaDisplay("Correct!");
        
        correctText.SetActive(true);

        // Thread.Sleep(2000);

        correctText.SetActive(false);

        DeleteAllGates();

        Debug.Log("Correct!");
        UpdateFormulaDisplay();

    }

    IEnumerator PauseAndSetValue(Switch input, string value, float delay)
    {
        yield return new WaitForSeconds(delay); // Pause without freezing the game
        input.setValue(value);
    }


    public void DeleteAllGates()
    {
        // List of tags to delete
        string[] tagsToDelete = { "And", "Or", "Not", "Probe", "Input" };

        foreach (string tag in tagsToDelete)
        {
            // Find all GameObjects with the current tag
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject obj in objectsWithTag)
            {
                // Handle special cleanup based on tag
                switch (tag)
                {
                    case "Input":
                        FormulaManager.removeInput(obj.GetComponent<Switch>());
                        break;

                    case "Probe":
                        FormulaManager.removeProbe();
                        break;
                }

                // Deregister all connections
                ConnectionPoint[] connectionPoints = obj.GetComponentsInChildren<ConnectionPoint>();
                foreach (ConnectionPoint connection in connectionPoints)
                {
                    if (connection.wire != null)
                    {
                        connection.wire.DeRegister(connection);
                    }
                }

                // Destroy the GameObject
                Destroy(obj);
            }
        }
    }

}