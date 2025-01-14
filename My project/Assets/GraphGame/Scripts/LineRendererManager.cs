using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]


public class LineRendererManager : MonoBehaviour
{
    private List<LineRenderer> lrs = new List<LineRenderer>();
    private LineRenderer clr;
    private Dictionary<(GameObject, GameObject), LineRenderer> connectedLines = new Dictionary<(GameObject, GameObject), LineRenderer>();
    [SerializeField] private GameObject lineRendererPrefab; // Prefab for LineRenderer

    private bool isDragging;
    private Vector3 endPoint;
    private GameObject startObject;
    private GameObject endObject;

    private DegreeOfNodes startNode;
    private DegreeOfNodes endNode;



    /*private void Start()
    {
        clr = GetComponent<LineRenderer>();
        clr.positionCount = 2;
    }
    */

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            // Check if a line was clicked
            foreach (var connection in connectedLines)
            {
                LineRenderer line = connection.Value;

                if (IsMouseOnLine(line, mousePosition))
                {
                    // Retrieve the connected GameObjects
                    GameObject startObject = connection.Key.Item1;
                    GameObject endObject = connection.Key.Item2;

                    // Remove the line
                    RemoveLine(startObject, endObject);
                    return;
                }
            }

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                startObject = hit.collider.gameObject;
                GameObject newLineRendererObject = Instantiate(lineRendererPrefab, transform.position, Quaternion.identity);
                clr = newLineRendererObject.GetComponent<LineRenderer>();
                clr.positionCount = 2;


                isDragging = true;
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0f;
                clr.SetPosition(0, mousePosition);
            }
        }

        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            clr.SetPosition(1, mousePosition);
            endPoint = mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;

            // Perform a 2D raycast from the endpoint of the line
            RaycastHit2D hit = Physics2D.Raycast(endPoint, Vector2.zero);
            // Check if the raycast hits an object with the required component or ID
            if (hit.collider != null)
            {
                endObject = hit.collider.gameObject;

                if (endObject != null && startObject != null && !connectedLines.ContainsKey((startObject, endObject)) && !connectedLines.ContainsKey((endObject, startObject)))
                {

                    connectedLines.Add((startObject, endObject), clr);

                    clr.SetPosition(0, startObject.transform.position);
                    clr.SetPosition(1, endObject.transform.position);

                    lrs.Add(clr);


                    startNode = startObject.GetComponent<DegreeOfNodes>();
                    endNode = endObject.GetComponent<DegreeOfNodes>();

                    startNode.IncreaseDegree();
                    endNode.IncreaseDegree();


                    Debug.Log("Correct Form!");
                    //this.enabled = false; // Disable this script if the match is correct
                }
                else
                {
                    Destroy(clr.gameObject);
                    Debug.Log("No valid endpoint, line not created.");
                }
            }
            else
            {
                Destroy(clr.gameObject);
                Debug.Log("No valid endpoint, line not created.");
            }

            // Reset variables
            startObject = null;
            endObject = null;
            clr = null;
            startNode = null;
            endNode = null;

        }

    }

    private bool IsMouseOnLine(LineRenderer line, Vector3 mousePosition)
    {
        // Check if the mouse is near any segment of the line
        for (int i = 0; i < line.positionCount - 1; i++)
        {
            Vector3 pointA = line.GetPosition(i);
            Vector3 pointB = line.GetPosition(i + 1);

            // Check if the mouse is close to the line segment
            if (Vector3.Distance(mousePosition, ClosestPointOnLine(pointA, pointB, mousePosition)) < 0.1f)
            {
                return true;
            }
        }
        return false;
    }

    private Vector3 ClosestPointOnLine(Vector3 pointA, Vector3 pointB, Vector3 target)
    {
        Vector3 line = pointB - pointA;
        float lineLength = line.magnitude;
        line.Normalize();

        float projectLength = Vector3.Dot(target - pointA, line);
        projectLength = Mathf.Clamp(projectLength, 0f, lineLength);

        return pointA + line * projectLength;
    }

    private void RemoveLine(GameObject startObject, GameObject endObject)
    {
        // Check for the connection in both directions
        if (connectedLines.TryGetValue((startObject, endObject), out LineRenderer line) ||
            connectedLines.TryGetValue((endObject, startObject), out line))
        {
            // Remove the connection from the dictionary
            connectedLines.Remove((startObject, endObject));
            connectedLines.Remove((endObject, startObject));

            startNode = startObject.GetComponent<DegreeOfNodes>();
            endNode = endObject.GetComponent<DegreeOfNodes>();

            if (startNode != null)
                startNode.DecreaseDegree();

            if (endNode != null)
                endNode.DecreaseDegree();

            // Destroy the LineRenderer GameObject
            Destroy(line.gameObject);

            Debug.Log($"Line removed between {startObject.name} and {endObject.name}");
        }
        else
        {
            Debug.Log("No line exists between these objects!");
        }
    }
    
}
