using UnityEngine;

public class NodeController : MonoBehaviour
{
    public string nodeName;  // Unique name for the node
    public SpriteRenderer spriteRenderer;
    public bool isSelected = false;  // Tracks if the node is selected
    public bool isStartNode = false; // Indicates if this is the start node
    public bool isEndNode = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    void OnMouseDown() {
        if (DijkstraManager.Instance == null)
        {
            Debug.LogError("DijkstraManager.Instance is null!");
            return;
        }

        
        // Force reassign the node to the correct DijkstraManager instance
        if (DijkstraManager.Instance != null)
        {
            DijkstraManager.Instance.OnNodeClicked(this);
            spriteRenderer.color = Color.yellow;  // Highlight the node

        }
    }


    public void ResetNode()
    {
        isSelected = false;
        spriteRenderer.color = Color.white;  // Reset to the default color
    }
}
