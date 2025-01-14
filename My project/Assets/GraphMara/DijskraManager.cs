
using System.Collections.Generic;
using UnityEngine;

public class DijkstraManager : MonoBehaviour
{
    public static DijkstraManager Instance;  // Singleton for global access

    public List<NodeController> selectedPath = new List<NodeController>();  // Player's selected path
    public GraphManager graphManager;   // Reference to the GraphManager
    public GameObject checkButton;      // Assign the Check button in the Inspector

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning($"Duplicate DijkstraManager detected! Instance Hash: {this.GetHashCode()}");
            // Do NOT destroy the duplicate yet; we will fix references first
            return;
        }

        Instance = this;
        Debug.Log($"DijkstraManager Singleton Instance set. Instance Hash: {Instance.GetHashCode()}");
    }



    public void OnNodeClicked(NodeController node)
    {
        selectedPath.Add(node);  // Add the clicked node to the path
        Debug.Log($"Added {node.nodeName} to the selected path.");  // Log added node
        Debug.Log("Selected Path:");
        foreach (var nod in selectedPath)
        {
            Debug.Log($"Node was added before in path:{nod.nodeName}");
        }
    }

    public void OnCheckButtonPressed() {
        Debug.Log("pressed button check");
        Debug.Log($"SelectedPath size: {selectedPath.Count}");
        if (selectedPath.Count == 0)
        {
            Debug.Log("No nodes selected.");
            return;
        }

        // Display the selected path
        Debug.Log("Selected Path:");
        foreach (var node in selectedPath)
        {
            Debug.Log(node.nodeName);
        }

        // Compute and display the correct path
        List<GameObject> correctPath = CalculateShortestPath();
        Debug.Log("Correct Path:");
        foreach (var node in correctPath)
        {
            Debug.Log(node.name);
        }

        // Validate the player's path
        ValidatePath(correctPath);
    }

    public void OnResetButtonPressed() {
        Debug.Log("Reset button pressed. Clearing selectedPath...");

        // Clear the selected path
        foreach (NodeController node in selectedPath)
        {
            node.ResetNode(); // Reset each node to its default state
        }

        selectedPath.Clear(); // Clear the path list
        Debug.Log("Game reset. SelectedPath cleared.");
    }

    private void ValidatePath(List<GameObject> correctPath)
    {
        if (selectedPath.Count != correctPath.Count)
        {
            Debug.Log("Invalid path: Incorrect number of nodes.");
            return;
        }

        for (int i = 0; i < selectedPath.Count; i++)
        {
            if (selectedPath[i].gameObject != correctPath[i])
            {
                Debug.Log("Invalid path: Nodes do not match the shortest path.");
                return;
            }
        }

        Debug.Log("Correct! You selected the shortest path!");
    }

    public List<GameObject> CalculateShortestPath()
    {
        Dictionary<GameObject, int> distances = new Dictionary<GameObject, int>();
        Dictionary<GameObject, GameObject> previousNodes = new Dictionary<GameObject, GameObject>();
        HashSet<GameObject> unvisited = new HashSet<GameObject>(graphManager.nodes);

        // Initialize distances
        foreach (GameObject node in graphManager.nodes)
        {
            distances[node] = int.MaxValue;
        }
        distances[graphManager.startNode] = 0;

        while (unvisited.Count > 0)
        {
            // Find the unvisited node with the smallest distance
            GameObject currentNode = null;
            foreach (GameObject node in unvisited)
            {
                if (currentNode == null || distances[node] < distances[currentNode])
                {
                    currentNode = node;
                }
            }

            if (currentNode == graphManager.finishNode)
            {
                break; // Reached the destination
            }

            unvisited.Remove(currentNode);

            // Update distances for neighbors
            foreach (var edge in graphManager.edges)
            {
                if (edge.Key.Item1 == currentNode || edge.Key.Item2 == currentNode)
                {
                    GameObject neighbor = edge.Key.Item1 == currentNode ? edge.Key.Item2 : edge.Key.Item1;
                    if (!unvisited.Contains(neighbor)) continue;

                    int newDist = distances[currentNode] + edge.Value;
                    if (newDist < distances[neighbor])
                    {
                        distances[neighbor] = newDist;
                        previousNodes[neighbor] = currentNode;
                    }
                }
            }
        }

        // Reconstruct the shortest path
        List<GameObject> path = new List<GameObject>();
        GameObject current = graphManager.finishNode;
        while (current != null)
        {
            path.Insert(0, current);
            previousNodes.TryGetValue(current, out current);
        }

        return path;
    }

    public void ResetGame()
    {
        // Reset all selected nodes
        foreach (NodeController node in selectedPath)
        {
            node.ResetNode();
        }

        selectedPath.Clear();
        Debug.Log("Game reset.");
    }
}
