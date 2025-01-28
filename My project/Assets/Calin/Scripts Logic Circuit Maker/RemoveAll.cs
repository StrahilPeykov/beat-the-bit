using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Events;
using Unity.VisualScripting;



public class RemoveAll : MonoBehaviour
{
    public Button removeButton;

    void Start()
    {
        // Add a listener to the button to call LoadLevel when clicked
        removeButton.onClick.AddListener(DeleteAllGates);
    }
    public void DeleteAllGates()
    {
        // List of tags to delete
        string[] tagsToDelete = { "And", "Or", "Not", "Probe", "InputLCM", "ProbeLCM" };

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