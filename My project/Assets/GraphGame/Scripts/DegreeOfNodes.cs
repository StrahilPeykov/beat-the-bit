using UnityEngine;

public class DegreeOfNodes : MonoBehaviour
{
    private int degree = 0;

    private void Start()
    {
        FindObjectOfType<DegreeOfNodesManager>().RegisterNode(this); // Register the node with the manager
    }

    private void OnDestroy()
    {
        FindObjectOfType<DegreeOfNodesManager>().UnregisterNode(this); // Unregister the node from the manager
    }

    public void IncreaseDegree()
    {
        degree++;
        Debug.Log($"{gameObject.name} degree increased to {degree}");
        FindObjectOfType<DegreeOfNodesManager>().CheckDegreesAndAddNode(); // Trigger degree check
    }

    public void DecreaseDegree()
    {
        degree = Mathf.Max(0, degree - 1);
        Debug.Log($"{gameObject.name} degree decreased to {degree}");
        FindObjectOfType<DegreeOfNodesManager>().CheckDegreesAndAddNode(); // Trigger degree check
    }

    public int GetDegree()
    {
        return degree;
    }
}