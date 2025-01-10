using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class GraphManager : MonoBehaviour
{
    public GameObject nodePrefab;        // Prefab for nodes
    public int numberOfNodes = 7;        // Total number of nodes to generate
    public Vector2 mapSize = new Vector2((float)1.2, (float)1.2); // Map boundaries

    public List<GameObject> nodes = new List<GameObject>(); // List to store generated nodes
    public Dictionary<(GameObject, GameObject), int> edges = new Dictionary<(GameObject, GameObject), int>(); // Edges with weights

    public GameObject startNode; // Reference to the start node
    public GameObject finishNode; // Reference to the finish node

    void Start()
    {
        GenerateGraph();           // Generate the graph
        AssignStartAndFinish();    // Assign start and finish nodes
    }

    void GenerateGraph() {
        nodes.Clear();
        edges.Clear();

        float minimumDistance = 2.0f; // Set minimum distance for both X and Y axes
        Debug.Log("Generating nodes...");

        // Step 1: Generate Nodes
        for (int i = 0; i < numberOfNodes; i++)
        {
            Vector2 randomPosition;
            bool positionIsValid;

            // Find a valid position with the minimum distance constraint
            do
            {
                positionIsValid = true;
                randomPosition = new Vector2(
                    Random.Range(-mapSize.x / 2, mapSize.x / 2),
                    Random.Range(-mapSize.y / 2, mapSize.y / 2)
                );

                // Check if the position satisfies the minimum distance constraint
                foreach (GameObject existingNode in nodes)
                {
                    Vector2 existingPosition = existingNode.transform.position;
                    if (Mathf.Abs(randomPosition.x - existingPosition.x) < minimumDistance &&
                        Mathf.Abs(randomPosition.y - existingPosition.y) < minimumDistance)
                    {
                        positionIsValid = false;
                        break;
                    }
                }
            } while (!positionIsValid);

            // Create and position the node
            GameObject newNode = Instantiate(nodePrefab, randomPosition, Quaternion.identity);
            newNode.name = $"Node_{i}";

            NodeController nodeController = newNode.GetComponent<NodeController>();
            if (nodeController != null)
            {
                nodeController.nodeName = newNode.name; // Assign the GameObject's name to nodeName
                Debug.Log($"Assigned nodeName: {nodeController.nodeName} to {newNode.name}, {mapSize}"); // Log the assignment
            }
            else
            {
                Debug.LogError("NodeController is missing from the NodePrefab!");
            }

            nodes.Add(newNode);
            Debug.Log($"Created {newNode.name} at {randomPosition}");
        }

        Debug.Log($"Generated {nodes.Count} nodes.");

        // Step 2: Ensure All Nodes Are Connected (MST)
        HashSet<GameObject> connectedNodes = new HashSet<GameObject>();
        connectedNodes.Add(nodes[0]); // Start with the first node

        while (connectedNodes.Count < nodes.Count)
        {
            GameObject currentNode = connectedNodes.ElementAt(Random.Range(0, connectedNodes.Count)); // Pick a random connected node
            GameObject newNode = nodes[Random.Range(0, nodes.Count)]; // Pick a random node

            if (!connectedNodes.Contains(newNode)) // Only connect if it's not already connected
            {
                int weight = Random.Range(1, 20);
                edges[(currentNode, newNode)] = weight;
                connectedNodes.Add(newNode);

                // Create the edge visually
                CreateEdge(currentNode, newNode, weight);
            }
        }

        Debug.Log("All nodes are now connected.");

        // Step 3: Add Additional Random Edges
        for (int i = 0; i < nodes.Count; i++)
        {
            for (int j = i + 1; j < nodes.Count; j++)
            {
                if (!edges.ContainsKey((nodes[i], nodes[j])) && !edges.ContainsKey((nodes[j], nodes[i])) && Random.Range(0, 2) == 1)
                {
                    int weight = Random.Range(1, 20);
                    edges[(nodes[i], nodes[j])] = weight;

                    // Create the edge visually
                    CreateEdge(nodes[i], nodes[j], weight);
                }
            }
        }

        Debug.Log($"Final graph has {edges.Count} edges.");
    }


    // void GenerateGraph() {
    //     nodes.Clear();
    //     edges.Clear();

    //     float minimumDistance = 3.0f; // Minimum distance between nodes
    //     int additionalEdges = 1;      // Add variability with extra edges per node

    //     // Step 1: Generate Well-Spaced Nodes
    //     for (int i = 0; i < numberOfNodes; i++)
    //     {
    //         Vector2 randomPosition;
    //         bool positionIsValid;

    //         // Keep trying to find a valid position for the node
    //         do
    //         {
    //             positionIsValid = true;
    //             randomPosition = new Vector2(
    //                 Random.Range(-mapSize.x / 2, mapSize.x / 2),
    //                 Random.Range(-mapSize.y / 2, mapSize.y / 2)
    //             );

    //             // Ensure the node is well-distanced from all other nodes
    //             foreach (GameObject existingNode in nodes)
    //             {
    //                 if (Vector2.Distance(randomPosition, existingNode.transform.position) < minimumDistance)
    //                 {
    //                     positionIsValid = false;
    //                     break;
    //                 }
    //             }
    //         }
    //         while (!positionIsValid);

    //         // Create the node at the valid position
    //         GameObject newNode = Instantiate(nodePrefab, randomPosition, Quaternion.identity);
    //         newNode.name = $"Node_{i}";
    //         nodes.Add(newNode);

    //         Debug.Log($"Created {newNode.name} at {randomPosition}");
    //     }

    //     // Step 2: Connect Nodes with at Least 2 Edges
    //     for (int i = 0; i < nodes.Count; i++)
    //     {
    //         GameObject currentNode = nodes[i];
    //         HashSet<GameObject> connectedNodes = new HashSet<GameObject>();

    //         // Connect to the 2 closest neighbors first
    //         List<GameObject> closestNodes = nodes
    //             .Where(node => node != currentNode)
    //             .OrderBy(node => Vector2.Distance(currentNode.transform.position, node.transform.position))
    //             .Take(2)
    //             .ToList();

    //         foreach (GameObject neighbor in closestNodes)
    //         {
    //             if (!connectedNodes.Contains(neighbor))
    //             {
    //                 int weight = Random.Range(1, 20); // Random weight
    //                 edges[(currentNode, neighbor)] = weight;

    //                 connectedNodes.Add(neighbor);

    //                 // Draw edge
    //                 CreateEdge(currentNode, neighbor, weight);
    //                 Debug.Log($"Connected {currentNode.name} to {neighbor.name} with weight {weight}");
    //             }
    //         }

    //         // Add random additional edges to introduce variability
    //         while (connectedNodes.Count < 2 + additionalEdges)
    //         {
    //             GameObject randomNode = nodes[Random.Range(0, nodes.Count)];
    //             if (randomNode != currentNode && !connectedNodes.Contains(randomNode))
    //             {
    //                 int weight = Random.Range(1, 20);
    //                 edges[(currentNode, randomNode)] = weight;

    //                 connectedNodes.Add(randomNode);

    //                 CreateEdge(currentNode, randomNode, weight);
    //                 Debug.Log($"Randomly connected {currentNode.name} to {randomNode.name} with weight {weight}");
    //             }
    //         }
    //     }

    //     Debug.Log($"Generated {edges.Count} edges.");

    //     // Step 3: Assign Start and Finish Nodes
    //     AssignStartAndFinish();
    // }

    void CreateEdge(GameObject startNode, GameObject endNode, int weight)
    {
        // Create a new GameObject for the edge
        GameObject edge = new GameObject("Edge");
        LineRenderer lineRenderer = edge.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startNode.transform.position);
        lineRenderer.SetPosition(1, endNode.transform.position);
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Use a basic shader
        lineRenderer.startColor = Color.blue;  // Set start color
        lineRenderer.endColor = Color.blue;    // Set end color

        // Create a new GameObject for the text
        GameObject weightText = new GameObject("WeightText");
        TextMeshPro textMeshPro = weightText.AddComponent<TextMeshPro>();

        // Customize TextMeshPro properties
        textMeshPro.text = weight.ToString();              // Set the text to the weight value
        textMeshPro.fontSize = 5;                          // Adjust font size
        textMeshPro.alignment = TextAlignmentOptions.Center; // Center the text alignment
        textMeshPro.color = Color.white;                   // Set text color

        // Position the text at the midpoint of the edge
        weightText.transform.position = (startNode.transform.position + endNode.transform.position) / 2;

        // Rotate the text to face the camera (optional)
        weightText.transform.rotation = Quaternion.identity;
    }

    void AssignStartAndFinish()
    {
        if (nodes.Count < 2)
        {
            Debug.LogError("Not enough nodes to assign start and finish points!");
            return;
        }

        // Randomly pick two distinct nodes
        int startIndex = Random.Range(0, nodes.Count);
        int finishIndex;
        do
        {
            finishIndex = Random.Range(0, nodes.Count);
        } while (finishIndex == startIndex); // Ensure start and finish are different

        startNode = nodes[startIndex];
        finishNode = nodes[finishIndex];

        // Mark and visually label the start and finish nodes
        MarkNodeAsStartOrFinish(startNode, "Start", Color.green);
        MarkNodeAsStartOrFinish(finishNode, "Finish", Color.red);

        Debug.Log($"Start Node: {startNode.name}, Finish Node: {finishNode.name}");
    }
    //     void AssignStartAndFinish() {
    //     if (nodes.Count < 2)
    //     {
    //         Debug.LogError("Not enough nodes to assign start and finish!");
    //         return;
    //     }

    //     // Pick start and finish nodes
    //     int startIndex = Random.Range(0, nodes.Count);
    //     int finishIndex;
    //     do
    //     {
    //         finishIndex = Random.Range(0, nodes.Count);
    //     } while (finishIndex == startIndex);

    //     startNode = nodes[startIndex];
    //     finishNode = nodes[finishIndex];

    //     // Highlight start and finish nodes
    //     NodeController startController = startNode.GetComponent<NodeController>();
    //     NodeController finishController = finishNode.GetComponent<NodeController>();

    //     if (startController != null)
    //     {
    //         startController.isStartNode = true;
    //         startNode.GetComponent<SpriteRenderer>().color = Color.green;

    //         Debug.Log($"Start Node: {startNode.name}");
    //     }

    //     if (finishController != null)
    //     {
    //         finishController.isEndNode = true;
    //         finishNode.GetComponent<SpriteRenderer>().color = Color.red;

    //         Debug.Log($"Finish Node: {finishNode.name}");
    //     }

    //     // Ensure multiple paths exist
    //     Debug.Log($"Ensuring multiple paths exist between {startNode.name} and {finishNode.name}...");
    // }

    void MarkNodeAsStartOrFinish(GameObject node, string label, Color color)
    {
        // Set the node color
        SpriteRenderer spriteRenderer = node.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }

        // Add a label above the node
        GameObject labelObject = new GameObject($"{label}Label");
        TextMeshPro textMeshPro = labelObject.AddComponent<TextMeshPro>();
        textMeshPro.text = label;
        textMeshPro.fontSize = 5;
        textMeshPro.alignment = TextAlignmentOptions.Center;
        textMeshPro.color = Color.white;
        labelObject.transform.position = node.transform.position + Vector3.up * 0.5f;
    }
}
