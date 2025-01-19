using System;
using UnityEngine;
public class NotGate : Movable, IGate
{
    public const string gateType = "Not";

    Wire inputWire;
    Wire outputWire;
    string id = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    [SerializeField] private SpriteRenderer spriteRenderer; // To render the sprite on the GameObject


    // 1
    [SerializeField]
    private Sprite not_1;

    // 0
    [SerializeField]
    private Sprite not_0;

    // s
    [SerializeField]
    private Sprite not_s;


    public bool FullyConnected { get; set; }

    public void DeRegisterWire(string id)
    {
        switch (id)
        {
            case "Input_A":
                inputWire = null;
                break;
            case "Output_A":
                outputWire = null;
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
            case "Output_A":
                outputWire = wire;
                break;
            default:
                throw new Exception("ID not supported");
        }
        CheckFullyConnected();
        UpdateLogic();
    }

    public void CheckFullyConnected()
    {
        if (inputWire != null && outputWire != null)
        {
            FullyConnected = true;
        } else
        {
            FullyConnected = false;
        }
    }

    public void UpdateLogic()
    {
        UpdateSprite();
        if (FullyConnected)
        {
            bool newSignal = !inputWire.signal;
            if (newSignal != outputWire.signal)
            {
                outputWire.UpdateSignal(newSignal, this);
            }
            
        }else if (outputWire != null && outputWire.signal)
            {
                outputWire.UpdateSignal(false , this);
            }
    }

    private void UpdateSprite()
    {
        Debug.Log("sprite update");
        string stateA;

        // spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogError("SpriteRenderer not found on AndGate object.", this);
                return;
            }
        }
        if (inputWire != null)
        {
            stateA = inputWire?.signal == true ? "1" : "0";
        }
        else
        {
            stateA = "s";
        }

        switch (stateA)
        {
            case "0":
                spriteRenderer.sprite = not_1;
                break;
            case "1":
                spriteRenderer.sprite = not_0;
                break;
            case "s":
                spriteRenderer.sprite = not_s;
                break;
            default:
                throw new Exception("Invalid sprite state");
        }
    }
    public string getId()
    {
        return this.id;
    }
}
