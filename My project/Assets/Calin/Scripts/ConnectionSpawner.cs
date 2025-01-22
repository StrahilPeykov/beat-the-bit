using UnityEngine;

public class ConnectionSpawner : MonoBehaviour
{
    // see if we have 0, 1 or 2 live connections
    enum State
    {
        ZERO_CONNECTED,
        ONE_CONNECTED,
        ALREADY_CONNECTED
    };

    State currentState = State.ZERO_CONNECTED;

    // start connection point which we initially don't have
    ConnectionPoint start = null;

    public bool makeWire = true;

    [SerializeField]
    private Wire wirePrefab;

    LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 3;
        lineRenderer.enabled = false;

        lineRenderer.startWidth = 0.12f;
        lineRenderer.endWidth = 0.12f;

        lineRenderer.startColor = new Color(Mathf.Pow(0.408f, 2.2f), Mathf.Pow(0.761f, 2.2f), Mathf.Pow(0.827f, 2.2f), 1.0f);
        lineRenderer.endColor = new Color(Mathf.Pow(0.408f, 2.2f), Mathf.Pow(0.761f, 2.2f), Mathf.Pow(0.827f, 2.2f), 1.0f);
    }
    void backToZeroConnected()
    {
        start = null;
        lineRenderer.enabled = false;
        currentState = State.ZERO_CONNECTED;
    }

    // this is called by the connection point and passes the pressed connection point (the one where the connection starts)
    public void RegisterConnection(ConnectionPoint connectionPoint)
    {
        // we cannot make a connection TO an output connection point
        // or to the same connection point
        // or to a connection point who already has a connection
        bool isBadEndConnection()
        {
            return connectionPoint.connectionType == ConnectionPoint.ConnectionType.OUTPUT
                || start == connectionPoint
                || connectionPoint.wire != null;
        }

        /**
        cannot start connection from an input connection point
        **/
        bool isBadStartConnection()
        {
            return connectionPoint.connectionType == ConnectionPoint.ConnectionType.INPUT;
        }

        // new wire basically
        if (currentState == State.ZERO_CONNECTED)
        {

            if (isBadStartConnection())
            {
                Debug.Log("aici");
                backToZeroConnected();
            }
            else
            {

                lineRenderer.enabled = true;
                start = connectionPoint;
                if (connectionPoint.wire != null)
                {
                    currentState = State.ALREADY_CONNECTED;
                }
                else
                {
                    currentState = State.ONE_CONNECTED;
                    UpdateLineRenderer();
                }
                // UpdateLineRenderer();

            }
        }
        else if (currentState == State.ONE_CONNECTED)
        {
            if (isBadEndConnection())
            {
                Debug.Log("aici");
                backToZeroConnected();
            }
            else
            {   // if (makeWire) {
                Debug.Log("aici");

                makeWire = !makeWire;
                Wire wire = Instantiate(wirePrefab, new Vector3(0, 0, 0), Quaternion.identity);
                wire.AddStartConnection(start);
                wire.AddEndConnection(connectionPoint);
                wire.Register();
                backToZeroConnected();
                // }

            }
        }
        else if (currentState == State.ALREADY_CONNECTED)
        {
            if (isBadEndConnection())
            {
                Debug.Log("aici");
                backToZeroConnected();
            }
            else
            {
                Debug.Log("aici");

                lineRenderer.enabled = false;
                lineRenderer.startWidth = 0.12f; // Adjust this value for desired thickness
                lineRenderer.endWidth = 0.12f;   // Match startWidth for consistent width

                lineRenderer.startColor = new Color(Mathf.Pow(0.709f, 2.2f), Mathf.Pow(0.322f, 2.2f), Mathf.Pow(0.325f, 2.2f), 1.0f);

                lineRenderer.endColor = new Color(Mathf.Pow(0.709f, 2.2f), Mathf.Pow(0.322f, 2.2f), Mathf.Pow(0.325f, 2.2f), 1.0f);
                
                Wire wire = start.wire;
                wire.AddEndConnection(connectionPoint);
                wire.Register();
                if (connectionPoint.wire == null)
                {
                    wire.AddEndConnection(connectionPoint);
                    wire.Register();
                }
                backToZeroConnected();
            }
        }
    }


    private void UpdateLineRenderer()
    {
        if (start != null) {

        
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 middlePos = new Vector3(start.transform.position.x, mousePos.y, 0);
        Vector3[] pos = { start.transform.position, middlePos, mousePos };
        lineRenderer.SetPositions(pos);
        lineRenderer.startWidth = 0.12f; // Adjust this value for desired thickness
        lineRenderer.endWidth = 0.12f;   // Match startWidth for consistent width

        lineRenderer.startColor = new Color(Mathf.Pow(0.709f, 2.2f), Mathf.Pow(0.322f, 2.2f), Mathf.Pow(0.325f, 2.2f), 1.0f);

        lineRenderer.endColor = new Color(Mathf.Pow(0.709f, 2.2f), Mathf.Pow(0.322f, 2.2f), Mathf.Pow(0.325f, 2.2f), 1.0f);

        } else {
            backToZeroConnected();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            backToZeroConnected();
        }
        if (currentState != State.ZERO_CONNECTED)
        {
            // Debug.Log("aici");
            UpdateLineRenderer();
        }
    }
}
// using UnityEngine;

// public class ConnectionSpawner : MonoBehaviour
// {
//     enum State
//     {
//         ZERO_CONNECTED,
//         ONE_CONNECTED,
//         ALREADY_CONNECTED
//     };

//     State currentState = State.ZERO_CONNECTED;

//     ConnectionPoint start = null;

//     [SerializeField]
//     private Wire wirePrefab;

//     LineRenderer lineRenderer;

//     private void Start()
//     {
//         lineRenderer = GetComponent<LineRenderer>();
//         lineRenderer.positionCount = 3;
//         lineRenderer.enabled = false;

//         lineRenderer.startWidth = 0.12f;
//         lineRenderer.endWidth = 0.12f;

//         lineRenderer.startColor = new Color(.167f, 0, 0.443f, 1);
//         lineRenderer.endColor = new Color(.167f, 0, 0.443f, 1);
//     }

//     void backToZeroConnected()
//     {
//         start = null;
//         lineRenderer.enabled = false;
//         currentState = State.ZERO_CONNECTED;
//     }

//     public void RegisterConnection(ConnectionPoint connectionPoint)
//     {
//         Debug.Log("registered connection");
//         bool isBadEndConnection()
//         {
//             return connectionPoint.connectionType == ConnectionPoint.ConnectionType.OUTPUT
//                 || start == connectionPoint
//                 || connectionPoint.wire != null;
//         }

//         bool isBadStartConnection()
//         {
//             return connectionPoint.connectionType == ConnectionPoint.ConnectionType.INPUT;
//         }

//         bool IsSameGateConnection(ConnectionPoint startPoint, ConnectionPoint endPoint)
//         {
//             return startPoint.logicGate == endPoint.logicGate;
//         }

//         if (currentState == State.ZERO_CONNECTED)
//         {
//             if (isBadStartConnection())
//             {
//                 backToZeroConnected();
//             }
//             else
//             {
//                 lineRenderer.enabled = true;
//                 start = connectionPoint;
//                 if (connectionPoint.wire != null)
//                 {
//                     currentState = State.ALREADY_CONNECTED;
//                 }
//                 else
//                 {
//                     currentState = State.ONE_CONNECTED;
//                 }
//                 UpdateLineRenderer();
//             }
//         }
//         else if (currentState == State.ONE_CONNECTED)
//         {
//             if (isBadEndConnection() || IsSameGateConnection(start, connectionPoint))
//             {
//                 Debug.LogWarning("Cannot connect to multiple points on the same logic gate.");
//                 backToZeroConnected();
//             }
//             else
//             {
//                 Wire wire = Instantiate(wirePrefab, new Vector3(0, 0, 0), Quaternion.identity);
//                 wire.AddStartConnection(start);
//                 wire.AddEndConnection(connectionPoint);
//                 wire.Register();
//                 backToZeroConnected();
//             }
//         }
//         else if (currentState == State.ALREADY_CONNECTED)
//         {
//             if (isBadEndConnection() || IsSameGateConnection(start, connectionPoint))
//             {
//                 Debug.LogWarning("Cannot connect to multiple points on the same logic gate.");
//                 backToZeroConnected();
//             }
//             else
//             {
//                 lineRenderer.enabled = false;

//                 Wire wire = start.wire;
//                 if (connectionPoint.wire == null)
//                 {
//                     wire.AddEndConnection(connectionPoint);
//                     wire.Register();
//                 }
//                 backToZeroConnected();
//             }
//         }
//     }

//     private void UpdateLineRenderer()
//     {
//         Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//         Vector2 middlePos = new Vector3(start.transform.position.x, mousePos.y, 0);
//         Vector3[] pos = { start.transform.position, middlePos, mousePos };
//         lineRenderer.SetPositions(pos);
//     }

//     private void Update()
//     {
//         if (Input.GetMouseButtonDown(1))
//         {
//             backToZeroConnected();
//         }
//         if (currentState != State.ZERO_CONNECTED)
//         {
//             UpdateLineRenderer();
//         }
//     }
// }
