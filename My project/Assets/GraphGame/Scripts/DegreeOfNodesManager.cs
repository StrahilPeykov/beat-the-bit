﻿using System.Collections.Generic;
using UnityEngine;

public class DegreeOfNodesManager : MonoBehaviour
{
    public PointSystem pointSystem;
    [SerializeField] private GameObject nodePrefab; // Prefab for new nodes
    private List<DegreeOfNodes> nodes = new List<DegreeOfNodes>(); // List of all nodes in the scene
    private int numberOfNodes = 0;
    private bool blocked = false;

    private int i = 1;

    public void RegisterNode(DegreeOfNodes node)
    {
        if (!nodes.Contains(node))
        {
            nodes.Add(node);
        }
    }

    public void UnregisterNode(DegreeOfNodes node)
    {
        if (nodes.Contains(node))
        {
            nodes.Remove(node);
        }
    }

    public void CheckDegreesAndAddNode()
    {
        foreach (DegreeOfNodes node in nodes)
        {
            if (node.GetDegree() % 2 == 0) // Check if any node has an even degree
            {
                Debug.Log("Not all nodes have odd degrees.");
                return;
            }
        }

        // If all nodes have odd degrees, add a new node
        if (!blocked)
        {
            AddNode();
        }
    }

    private void AddNode()
    {
        if (numberOfNodes == 4)
        {
            Vector3 lastPosition = GenerateRandomPosition();
            GameObject lastNodeObject = Instantiate(nodePrefab, lastPosition, Quaternion.identity);

            DegreeOfNodes lastNode = lastNodeObject.GetComponent<DegreeOfNodes>();
            if (lastNode != null)
            {
                RegisterNode(lastNode); // Add the new node to the list

                Debug.Log("New node added at position: " + lastPosition);
            }

            blocked = true;

            pointSystem.AddPoints(10);



        }
        else
        {

            Vector3 newPosition = GenerateRandomPosition();
            GameObject newNodeObject = Instantiate(nodePrefab, newPosition, Quaternion.identity);

            DegreeOfNodes newNode = newNodeObject.GetComponent<DegreeOfNodes>();
            if (newNode != null)
            {
                RegisterNode(newNode); // Add the new node to the list
                ++numberOfNodes;
                Debug.Log("New node added at position: " + newPosition);
            }
            else
            {
                Debug.LogError("DegreeOfNodes component missing on the prefab!");
            }


            Vector3 newPosition_2 = GenerateRandomPosition();
            GameObject newNodeObject_2 = Instantiate(nodePrefab, newPosition_2, Quaternion.identity);

            DegreeOfNodes newNode_2 = newNodeObject_2.GetComponent<DegreeOfNodes>();
            if (newNode_2 != null)
            {
                RegisterNode(newNode_2); // Add the new node to the list
                ++numberOfNodes;
                Debug.Log("New node added at position: " + newPosition);
            }
            else
            {
                Debug.LogError("DegreeOfNodes component missing on the prefab!");
            }

            i += 1;

            pointSystem.AddPoints(20);
        }

    }

    private Vector3 GenerateRandomPosition()
    {
        int radius = 2;
        // Generate a random angle in radians (0 to 2π)
        float angle = Random.Range(0f, Mathf.PI * 2);

        // Generate a random distance from the center (0 to radius)
        float distance = Random.Range(0f, radius);

        // Calculate the x and y coordinates
        float x = Mathf.Cos(angle) * distance;
        float y = Mathf.Sin(angle) * distance;

        // Return the position as a Vector3
        return new Vector3(x, y, 0f);
    }
}