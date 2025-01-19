using UnityEngine;
using UnityEngine.EventSystems;

public class ConnectionPoint : MonoBehaviour, IPointerClickHandler
{
    public string id;

    public IGate logicGate;

    public Wire wire = null;

    ConnectionSpawner connectionSpawner;

    public enum ConnectionType
    {
        INPUT,
        OUTPUT
    };

    public ConnectionType connectionType;

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (connectionType == ConnectionType.INPUT)
        {
            spriteRenderer.color = Color.white;
        }
        else
        {
            spriteRenderer.color = Color.grey;
        }

        connectionSpawner = FindObjectOfType<ConnectionSpawner>();
        logicGate = GetComponentInParent<IGate>();
    }

    public void RegisterWire(Wire wire)
    {
        
        this.wire = wire;
        logicGate.RegisterWire(id, wire);
    }

    internal void DeRegisterWire()
    {
        // connectionSpawner.makeWire = true;
        logicGate.DeRegisterWire(this.id);
        wire = null;
        // logicGate.DeRegisterWire(id);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (wire != null)
            {
                wire.DeRegister(this);
                Debug.Log("deregistered cp");
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            connectionSpawner.RegisterConnection(this);
        }
    }

    // public bool IsOtherInputConnected()
    // {
    //     ConnectionPoint[] allPoints = logicGate.GetComponentsInChildren<ConnectionPoint>();

    //     foreach (ConnectionPoint point in allPoints)
    //     {
            
    //         if (point != this && point.connectionType == ConnectionType.INPUT)
    //         {
                
    //             if (point.wire != null)
    //             {
    //                 return true;
    //             }
    //         }
    //     }

    //     // No other input is connected
    //     return false;
    // }

}

