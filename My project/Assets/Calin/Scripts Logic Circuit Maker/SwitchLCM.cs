using System;
using UnityEngine;

public class SwitchLCM : Movable, IGate
{
    public const string gateType = "Input";

    Wire outputWire;

    [SerializeField]
    bool signal = false;

    [SerializeField]
    private Sprite onSprite; // Sprite when signal is true

    [SerializeField]
    private Sprite offSprite; // Sprite when signal is false

    string id = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

    SpriteRenderer spriteRenderer;

    public bool FullyConnected { get; set; }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void DeRegisterWire(string id)
    {
        switch (id)
        {
            case "Output_A":
                outputWire = null;
                // Debug.LogWarning($"Wire with ID {outputWire.getId()} is deregistered.");
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
            case "Output_A":
                outputWire = wire;
                Debug.LogWarning($"Wire with ID {wire.getId()} is registered.");
                break;
            default:
                throw new Exception("ID not supported");
        }
        CheckFullyConnected();
        UpdateLogic();
    }
    public void CheckFullyConnected()
    {
        if (outputWire != null)
        {
            FullyConnected = true;
        }
        else
        {
            FullyConnected = false;
        }
    }

    public void UpdateLogic()
    {
        updateSprite();
        if (FullyConnected && outputWire.signal != signal)
        {
            outputWire.UpdateSignal(signal, this);
        }
    }

    

    public void OnMouseUpAsButton()
    {
        signal = !signal;
        UpdateLogic();


    }

    public void updateSprite()
    {
        if (signal && onSprite != null)
        {
            spriteRenderer.sprite = onSprite;
        }
        else if (!signal && offSprite != null)
        {
            spriteRenderer.sprite = offSprite;
        }
    }

    public string getId()
    {
        return this.id;
    }

   

    public void setValue(string val)
    {
        if (val == "1")
        {
            signal = true;
        }
        else
        {
            signal = false;
        }

        CheckFullyConnected();
        UpdateLogic();

    }
}

