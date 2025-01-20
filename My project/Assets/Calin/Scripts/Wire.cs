using System;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{

    public string id = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    public ConnectionPoint startConnectionPoint;

    [SerializeField]
    public List<Tuple<ConnectionPoint, LineRenderer>> endConnectionPoints = new List<Tuple<ConnectionPoint, LineRenderer>>();   

    public bool render = false;

    public bool signal = false;

    // public HashSet<IGate> registeredGates = new HashSet<IGate>();

    public Dictionary<string, IGate> registeredGates= new Dictionary<string, IGate>();

    public Dictionary<string, int> registeredNrOfGates= new Dictionary<string, int>();


    [SerializeField]
    public GameObject WireChildPrefab;

    void Update()
    {
        if (render)
        {
            Vector3 startPos = startConnectionPoint.transform.position;
            foreach (Tuple<ConnectionPoint, LineRenderer> tuple in endConnectionPoints)
            {
                LineRenderer lineRenderer = tuple.Item2;
                ConnectionPoint connectionPoint = tuple.Item1;
                Vector3 endPos = connectionPoint.transform.position;
                Vector3 middlePoint = new Vector3(startPos.x, endPos.y);
                Vector3[] pos = { startPos, middlePoint, endPos };
                lineRenderer.SetPositions(pos);
                lineRenderer.startWidth = 0.12f; 
                lineRenderer.endWidth = 0.12f;  

                // lineRenderer.startColor = new Color(.167f, 0, 0.443f, 1);
                // lineRenderer.startColor = new Color(Mathf.Pow(0.709f, 2.2f), Mathf.Pow(0.322f, 2.2f), Mathf.Pow(0.325f, 2.2f), 1.0f);
                lineRenderer.startColor = new Color(Mathf.Pow(0.408f, 2.2f), Mathf.Pow(0.761f, 2.2f), Mathf.Pow(0.827f, 2.2f), 1.0f);
                lineRenderer.endColor = new Color(Mathf.Pow(0.408f, 2.2f), Mathf.Pow(0.761f, 2.2f), Mathf.Pow(0.827f, 2.2f),   1.0f);
            }
        }
    }

    public void AddEndConnection(ConnectionPoint connectionPoint)
    {
        GameObject wireChild = Instantiate(WireChildPrefab, Vector3.zero, Quaternion.identity);
        wireChild.transform.SetParent(gameObject.transform);
        LineRenderer lineRenderer = wireChild.GetComponent<LineRenderer>();
        endConnectionPoints.Add(new Tuple<ConnectionPoint, LineRenderer>(connectionPoint, lineRenderer));
    }

    public void AddStartConnection(ConnectionPoint connectionPoint)
    {
        startConnectionPoint = connectionPoint;
    }

    public void Register()
    {
        registeredGates.Clear();
        registeredNrOfGates.Clear();
        // registeredGates.Add(startConnectionPoint.logicGate);
        string logicGateId = startConnectionPoint.logicGate.getId();

        // if(!registeredGates.ContainsKey(logicGateId)) {
        //     registeredGates[logicGateId] = startConnectionPoint.logicGate;
        //     registeredNrOfGates[logicGateId] = 1;
        //     Debug.Log("Registered start logic gate " + logicGateId + " " + registeredNrOfGates[logicGateId]);
        // }

        registeredGates[logicGateId] = startConnectionPoint.logicGate;
        registeredNrOfGates[logicGateId] = 1;
        //Debug.Log("Registered start logic gate " + logicGateId + " " + registeredNrOfGates[logicGateId]);




        Vector3 startPos = startConnectionPoint.transform.position;
        foreach (Tuple<ConnectionPoint, LineRenderer> tuple in endConnectionPoints)
        {
            LineRenderer lineRenderer = tuple.Item2;
            ConnectionPoint connectionPoint = tuple.Item1;
            Vector3 endPos = connectionPoint.transform.position;
            Vector3 middlePoint = new Vector3(startPos.x, endPos.y);
            Vector3[] pos = { startPos, middlePoint, endPos };
            lineRenderer.SetPositions(pos);
            lineRenderer.enabled = true;

            // registeredGates.Add(tuple.Item1.logicGate);
            registeredGates[tuple.Item1.logicGate.getId()] = tuple.Item1.logicGate;

            if (registeredNrOfGates.ContainsKey(tuple.Item1.logicGate.getId()))
            {
                // if (registeredNrOfGates[tuple.Item1.logicGate.getId()] <= 1) {
                //     registeredNrOfGates[tuple.Item1.logicGate.getId()]++;
                //     Debug.Log("Registered end connection " + tuple.Item1.logicGate.getId() + " " + registeredNrOfGates[tuple.Item1.logicGate.getId()]);
                // } 

                registeredNrOfGates[tuple.Item1.logicGate.getId()]++;
                // Debug.Log("Registered end connection " + tuple.Item1.logicGate.getId() + " " + registeredNrOfGates[tuple.Item1.logicGate.getId()]);

            } else
            {
                registeredNrOfGates[tuple.Item1.logicGate.getId()] = 1;
                // Debug.Log("Registered end connection " + tuple.Item1.logicGate.getId() + " " + registeredNrOfGates[tuple.Item1.logicGate.getId()]);
            }

            tuple.Item1.RegisterWire(this);
        }
        startConnectionPoint.RegisterWire(this);
        render = true;
        UpdateSignal();
    }

    public void DeRegister(ConnectionPoint deregisterConnectionPoint)
    {
        if (startConnectionPoint == deregisterConnectionPoint)
        {
            startConnectionPoint.DeRegisterWire();
            endConnectionPoints.ForEach(tuple => tuple.Item1.DeRegisterWire());
            Destroy(gameObject);
        } else {

            Destroy(endConnectionPoints.Find(item => item.Item1 == deregisterConnectionPoint).Item2.gameObject);
            endConnectionPoints.RemoveAll(item => item.Item1 == deregisterConnectionPoint);
            deregisterConnectionPoint.DeRegisterWire();
            // registeredGates.Remove(deregisterConnectionPoint.logicGate);

            // Debug.Log("inainte still registered = " + registeredGates.Count);

            if (registeredNrOfGates.ContainsKey(deregisterConnectionPoint.logicGate.getId()))
            {
                // Debug.Log("1. intra n if si sunt atatea registrate " + registeredNrOfGates[deregisterConnectionPoint.logicGate.getId()]);

                registeredNrOfGates[deregisterConnectionPoint.logicGate.getId()]--;

                // Debug.Log("2. intra n if si sunt atatea registrate " + registeredNrOfGates[deregisterConnectionPoint.logicGate.getId()]);

                if (registeredNrOfGates[deregisterConnectionPoint.logicGate.getId()] == 0) {
                    registeredGates.Remove(deregisterConnectionPoint.logicGate.getId());
                }
            }
            // else
            // {
            //     registeredNrOfGates[deregisterConnectionPoint.logicGate.getId()] = 1;
            // }

            // Debug.Log("dupa still registered = " + registeredGates.Count);

            if (registeredGates.Count == 1)
            {
                // Debug.Log("byebye");
                startConnectionPoint.DeRegisterWire();
                Destroy(gameObject);

            }
            // Debug.Log(registeredGates.Count);
        }


    }

    // maybe here?
    public void UpdateSignal()
    {
        // foreach (IGate logicGate in registeredGates)
        // {
        //     logicGate.UpdateLogic();
        // }

        foreach(var kvp in registeredGates) {
            kvp.Value.UpdateLogic();
        }
    }

    public void UpdateSignal(bool newSignal, IGate currentLogicGate)
    { 
        signal = newSignal;
        // foreach (IGate logicGate in registeredGates)
        // {
        //     if (logicGate != currentLogicGate)
        //     {
        //         logicGate.UpdateLogic();
        //     }
        // }

        foreach (var kvp in registeredGates)
        {
            if(kvp.Value.getId() != currentLogicGate.getId()) {
                kvp.Value.UpdateLogic();

            }
        }

    }

    public string getId() {
        return this.id;
    }
}


// using System;
// using System.Collections.Generic;
// using UnityEngine;

// public class Wire : MonoBehaviour
// {
//     public ConnectionPoint startConnectionPoint;

//     [SerializeField]
//     public List<Tuple<ConnectionPoint, LineRenderer>> endConnectionPoints = new List<Tuple<ConnectionPoint, LineRenderer>>();

//     public bool render = false;

//     public bool signal = false;

//     public HashSet<IGate> registeredGates = new HashSet<IGate>();

//     [SerializeField]
//     public GameObject WireChildPrefab;

//     void Update()
//     {
//         if (render)
//         {
//             Vector3 startPos = startConnectionPoint.transform.position;
//             foreach (Tuple<ConnectionPoint, LineRenderer> tuple in endConnectionPoints)
//             {
//                 LineRenderer lineRenderer = tuple.Item2;
//                 ConnectionPoint connectionPoint = tuple.Item1;
//                 Vector3 endPos = connectionPoint.transform.position;
//                 Vector3 middlePoint = new Vector3(startPos.x, endPos.y);
//                 Vector3[] pos = { startPos, middlePoint, endPos };
//                 lineRenderer.SetPositions(pos);
//             }
//         }
//     }

//     public void AddEndConnection(ConnectionPoint connectionPoint)
//     {
//         GameObject wireChild = Instantiate(WireChildPrefab, Vector3.zero, Quaternion.identity);
//         wireChild.transform.SetParent(gameObject.transform);
//         LineRenderer lineRenderer = wireChild.GetComponent<LineRenderer>();
//         endConnectionPoints.Add(new Tuple<ConnectionPoint, LineRenderer>(connectionPoint, lineRenderer));
//     }

//     public void AddStartConnection(ConnectionPoint connectionPoint)
//     {
//         startConnectionPoint = connectionPoint;
//     }

//     public void Register()
//     {
//         registeredGates.Add(startConnectionPoint.logicGate);

//         Vector3 startPos = startConnectionPoint.transform.position;
//         foreach (Tuple<ConnectionPoint, LineRenderer> tuple in endConnectionPoints)
//         {
//             LineRenderer lineRenderer = tuple.Item2;
//             ConnectionPoint connectionPoint = tuple.Item1;
//             Vector3 endPos = connectionPoint.transform.position;
//             Vector3 middlePoint = new Vector3(startPos.x, endPos.y);
//             Vector3[] pos = { startPos, middlePoint, endPos };
//             lineRenderer.SetPositions(pos);
//             lineRenderer.enabled = true;

//             registeredGates.Add(tuple.Item1.logicGate);
//             tuple.Item1.RegisterWire(this);
//         }
//         startConnectionPoint.RegisterWire(this);
//         render = true;
//         UpdateSignal();
//     }

//     public void DeRegister(ConnectionPoint deregisterConnectionPoint)
//     {
//         if (startConnectionPoint == deregisterConnectionPoint)
//         {
//             startConnectionPoint.DeRegisterWire();
//             endConnectionPoints.ForEach(tuple => tuple.Item1.DeRegisterWire());
//             Destroy(gameObject);
//         }
//         else
//         {
//             Destroy(endConnectionPoints.Find(item => item.Item1 == deregisterConnectionPoint).Item2.gameObject);
//             endConnectionPoints.RemoveAll(item => item.Item1 == deregisterConnectionPoint);
//             deregisterConnectionPoint.DeRegisterWire();
//             registeredGates.Remove(deregisterConnectionPoint.logicGate);
//             if (registeredGates.Count == 0)
//             {
//                 Destroy(gameObject);
//             }
//         }
//     }

//     public void UpdateSignal()
//     {
//         foreach (IGate logicGate in registeredGates)
//         {
//             logicGate.UpdateLogic();
//         }
//     }

//     public void UpdateSignal(bool newSignal, IGate currentLogicGate)
//     {
//         signal = newSignal;
//         foreach (IGate logicGate in registeredGates)
//         {
//             if (logicGate != currentLogicGate)
//             {
//                 logicGate.UpdateLogic();
//             }
//         }
//     }

//     public string getId()
//     {
//         return "boring";
//     }
// }