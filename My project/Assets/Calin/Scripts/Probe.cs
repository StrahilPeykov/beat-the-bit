using System;
using UnityEngine;

public class Probe : Movable, IGate
{
    Wire inputWire;

    string id = "probe" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

    [SerializeField]
    bool signal = false;
    public bool FullyConnected { get; set; }

    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateColor();
    }
    public void CheckFullyConnected()
    {
        if (inputWire != null)
        {
            FullyConnected = true;
        }
        else
        {
            FullyConnected = false;
        }
    }

    public void DeRegisterWire(string id)
    {
        switch (id.Split('0')[0])
        {
            case "Input_A":
                inputWire = null;
                break;
            default:
                throw new Exception("ID not supported");
        }
        CheckFullyConnected();
        UpdateLogic();
    }

    public void RegisterWire(string id, Wire wire)
    {
        switch (id)
        {
            case "Input_A":
                inputWire = wire;
                break;
            default:
                throw new Exception("ID not supported");
        }
        CheckFullyConnected();
        UpdateLogic();
    }

    public void UpdateLogic()
    {
        if (FullyConnected)
        {
            signal = inputWire.signal;
        }
        else
        {
            signal = false;
        }
        UpdateColor();
    }

    void UpdateColor()
    {
        if (signal)
        {
            spriteRenderer.color = Color.yellow;
        }
        else
        {
            spriteRenderer.color = Color.grey;
        }
    }

    public string getId()
    {
        return this.id;
    }

    public int getSignal()
    {
        return signal ? 1 : 0;
    }

}
