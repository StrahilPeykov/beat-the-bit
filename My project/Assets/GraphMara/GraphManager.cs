using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using DialogueEditor;

public class GraphManager : MonoBehaviour
{
    public GameObject nodePrefab;        // Prefab for nodes
    public int numberOfNodes = 3;        // Total number of nodes to generate
    public Vector2 mapSize = new Vector2((float)6.6, (float)3.6); // Map boundaries

    public List<GameObject> nodes = new List<GameObject>(); // List to store generated nodes
    public Dictionary<(GameObject, GameObject), int> edges = new Dictionary<(GameObject, GameObject), int>(); // Edges with weights

    public GameObject startNode; // Reference to the start node
    public GameObject finishNode; // Reference to the finish node
    private List<GameObject> extraObjects = new List<GameObject>();

    public NPCConversation myConv;
    public GameObject resetButton;
    public GameObject regenerateButton;

    public GameObject checkButton;



    void Start()
    {
        resetButton.gameObject.SetActive(false);
        regenerateButton.gameObject.SetActive(false);
        checkButton.gameObject.SetActive(false);
       // GenerateGraph();           // Generate the graph
       
        
    }

    public void StartConversation() {
        ConversationManager.Instance.StartConversation(myConv);
    }

    // void GenerateGraph() {
    //     nodes.Clear();
    //     edges.Clear();

    //     float minimumDistance = 0.5f; // Set minimum distance for both X and Y axes
    //     Debug.Log("Generating nodes...");

    //     // Step 1: Generate Nodes
    //     for (int i = 0; i < numberOfNodes; i++)
    //     {
    //         Vector2 randomPosition;
    //         bool positionIsValid;

    //         // Find a valid position with the minimum distance constraint
    //         do
    //         {
    //             positionIsValid = true;
    //             randomPosition = new Vector2(
    //                 Random.Range(-mapSize.x / 2, mapSize.x / 2),
    //                 Random.Range(-mapSize.y / 2, mapSize.y / 2)
    //             );

    //             // Check if the position satisfies the minimum distance constraint
    //             foreach (GameObject existingNode in nodes)
    //             {
    //                 Vector2 existingPosition = existingNode.transform.position;
    //                 if (Mathf.Abs(randomPosition.x - existingPosition.x) < minimumDistance &&
    //                     Mathf.Abs(randomPosition.y - existingPosition.y) < minimumDistance)
    //                 {
    //                     positionIsValid = false;
    //                     break;
    //                 }
    //             }
    //         } while (!positionIsValid);

    //         // Create and position the node
    //         GameObject newNode = Instantiate(nodePrefab, randomPosition, Quaternion.identity);
    //         newNode.name = $"Node_{i}";

    //         NodeController nodeController = newNode.GetComponent<NodeController>();
    //         if (nodeController != null)
    //         {
    //             nodeController.nodeName = newNode.name; // Assign the GameObject's name to nodeName
    //             Debug.Log($"Assigned nodeName: {nodeController.nodeName} to {newNode.name}, {mapSize}"); // Log the assignment
    //         }
    //         else
    //         {
    //             Debug.LogError("NodeController is missing from the NodePrefab!");
    //         }

    //         nodes.Add(newNode);
    //         Debug.Log($"Created {newNode.name} at {randomPosition}");
    //     }

    //     Debug.Log($"Generated {nodes.Count} nodes.");

    //     // Step 2: Ensure All Nodes Are Connected (MST)
    //     HashSet<GameObject> connectedNodes = new HashSet<GameObject>();
    //     connectedNodes.Add(nodes[0]); // Start with the first node

    //     while (connectedNodes.Count < nodes.Count)
    //     {
    //         GameObject currentNode = connectedNodes.ElementAt(Random.Range(0, connectedNodes.Count)); // Pick a random connected node
    //         GameObject newNode = nodes[Random.Range(0, nodes.Count)]; // Pick a random node

    //         if (!connectedNodes.Contains(newNode)) // Only connect if it's not already connected
    //         {
    //             int weight = Random.Range(1, 20);
    //             edges[(currentNode, newNode)] = weight;
    //             connectedNodes.Add(newNode);

    //             // Create the edge visually
    //             CreateEdge(currentNode, newNode, weight);
    //         }
    //     }

    //     Debug.Log("All nodes are now connected.");

    //     // Step 3: Add Additional Random Edges
    //     for (int i = 0; i < nodes.Count; i++)
    //     {
    //         for (int j = i + 1; j < nodes.Count; j++)
    //         {
    //             if (!edges.ContainsKey((nodes[i], nodes[j])) && !edges.ContainsKey((nodes[j], nodes[i])) && Random.Range(0, 2) == 1)
    //             {
    //                 int weight = Random.Range(1, 20);
    //                 edges[(nodes[i], nodes[j])] = weight;

    //                 // Create the edge visually
    //                 CreateEdge(nodes[i], nodes[j], weight);
    //             }
    //         }
    //     }

    //     Debug.Log($"Final graph has {edges.Count} edges.");
    // }
    public void GenerateGraph() {
        nodes.Clear();
        edges.Clear();

        float minimumDistance = 1.0f; // Set minimum distance for both X and Y axes
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
                    Random.Range(-mapSize.y / 2 + 0.2f, mapSize.y / 2)
                );

                // Check if the position satisfies the minimum distance constraint
                foreach (GameObject existingNode in nodes)
                {
                    Vector2 existingPosition = existingNode.transform.position;
                    if (Vector2.Distance(randomPosition, existingPosition) < minimumDistance)
                    {
                        positionIsValid = false;
                        break;
                    }
                }
            } while (!positionIsValid);

            // Create and position the node
            GameObject newNode = Instantiate(nodePrefab, randomPosition, Quaternion.identity);
            newNode.name = $"Node_{i}";
            newNode.GetComponent<Renderer>().sortingOrder=5;

            NodeController nodeController = newNode.GetComponent<NodeController>();
            if (nodeController != null)
            {
                nodeController.nodeName = newNode.name; // Assign the GameObject's name to nodeName
                Debug.Log($"Assigned nodeName: {nodeController.nodeName} to {newNode.name}");
            }
            else
            {
                Debug.LogError("NodeController is missing from the NodePrefab!");
            }

            nodes.Add(newNode);
            Debug.Log($"Created {newNode.name} at {randomPosition}");
        }

        Debug.Log($"Generated {nodes.Count} nodes.");

        // Step 2: Assign Start and Finish Nodes
        AssignStartAndFinish();

        // Step 3: Ensure All Nodes Are Connected (MST)
        HashSet<GameObject> connectedNodes = new HashSet<GameObject>();
        connectedNodes.Add(nodes[0]); // Start with the first node

        while (connectedNodes.Count < nodes.Count)
        {
            GameObject currentNode = connectedNodes.ElementAt(Random.Range(0, connectedNodes.Count)); // Pick a random connected node
            GameObject newNode = nodes[Random.Range(0, nodes.Count)]; // Pick a random node

            if (!connectedNodes.Contains(newNode)) // Only connect if it's not already connected
            {
                // Ensure there is no direct edge between startNode and finishNode
                if ((currentNode == startNode && newNode == finishNode) || 
                    (currentNode == finishNode && newNode == startNode))
                {
                    continue;
                }

                int weight = Random.Range(1, 20);
                edges[(currentNode, newNode)] = weight;
                connectedNodes.Add(newNode);

                // Create the edge visually
                CreateEdge(currentNode, newNode, weight);
            }
        }

        Debug.Log("All nodes are now connected.");

        // Step 4: Add Additional Random Edges
        for (int i = 0; i < nodes.Count; i++)
        {
            for (int j = i + 1; j < nodes.Count; j++)
            {
                if (!edges.ContainsKey((nodes[i], nodes[j])) && !edges.ContainsKey((nodes[j], nodes[i])) && Random.Range(0, 2) == 1)
                {
                    // Ensure there is no direct edge between startNode and finishNode
                    if ((nodes[i] == startNode && nodes[j] == finishNode) || 
                        (nodes[i] == finishNode && nodes[j] == startNode))
                    {
                        continue;
                    }

                    int weight = Random.Range(1, 20);
                    edges[(nodes[i], nodes[j])] = weight;

                    // Create the edge visually
                    CreateEdge(nodes[i], nodes[j], weight);
                }
            }
        }

        Debug.Log($"Final graph has {edges.Count} edges.");
    }


    void CreateEdge(GameObject startNode, GameObject endNode, int weight)
    {
        // Create a new GameObject for the edge
        GameObject edge = new GameObject("Edge");
        Color customBeige = new Color(0.823f, 0.7058f, 0.5490f);
        LineRenderer lineRenderer = edge.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startNode.transform.position);
        lineRenderer.SetPosition(1, endNode.transform.position);
        lineRenderer.startWidth = 0.03f;
        lineRenderer.endWidth = 0.03f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Use a basic shader
        lineRenderer.startColor = customBeige;  // Set start color
        lineRenderer.endColor = customBeige;    // Set end color
        lineRenderer.GetComponent<Renderer>().sortingOrder = 0;
        extraObjects.Add(edge);

        

        // Create a new GameObject for the text
        GameObject weightText = new GameObject("WeightText");
        TextMeshPro textMeshPro = weightText.AddComponent<TextMeshPro>();
        

        // Customize TextMeshPro properties
        textMeshPro.text = weight.ToString();              // Set the text to the weight value
        textMeshPro.fontSize = 2;                          // Adjust font size
        textMeshPro.alignment = TextAlignmentOptions.Center; // Center the text alignment
        textMeshPro.color = Color.white;  
        textMeshPro.GetComponent<Renderer>().sortingOrder = 1;            // Set text color
        extraObjects.Add(weightText);

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
        textMeshPro.fontSize = 2;
        textMeshPro.alignment = TextAlignmentOptions.Center;
        textMeshPro.color = Color.white;
        labelObject.transform.position = node.transform.position + Vector3.up * 0.1f;
        extraObjects.Add(labelObject);
    }

    public void RegenerateGame() {
        Debug.Log("Regenerating the game...");

         // Clear the selected path in DijkstraManager or similar logic
        if (DijkstraManager.Instance != null)
        {
            DijkstraManager.Instance.ClearSelectedPath(); // Clears references to old nodes
        }

        // Destroy all existing nodes
        foreach (GameObject node in nodes)
        {
            Destroy(node);
        }

        // Destroy all edge GameObjects
        foreach (GameObject edge in extraObjects)
        {
            Destroy(edge);
        }
        extraObjects.Clear(); // Clear the list of edge GameObjects

        // Clear the nodes and edges collections
        nodes.Clear();
        edges.Clear();

        // Generate a new graph
        GenerateGraph();

        Debug.Log("New game generated.");
    }
}
