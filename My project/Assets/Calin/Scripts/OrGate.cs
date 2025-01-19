using System;
using UnityEngine;
public class OrGate : Movable, IGate
{
    public const string gateType = "Or";

    Wire inputWireA;
    Wire inputWireB;
    Wire outputWire;
    string id = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

    [SerializeField] private SpriteRenderer spriteRenderer; // To render the sprite on the GameObject
    // 11
    [SerializeField]
    private Sprite or_11;

    // 00
    [SerializeField]
    private Sprite or_00;

    // 10
    [SerializeField]
    private Sprite or_10;

    // 01
    [SerializeField]
    private Sprite or_01;

    // 1s
    [SerializeField]
    private Sprite or_1s;

    // 0s
    [SerializeField]
    private Sprite or_0s;

    // s1
    [SerializeField]
    private Sprite or_s1;

    // s0
    [SerializeField]
    private Sprite or_s0;

    // ss
    [SerializeField]
    private Sprite or_ss;


    public bool FullyConnected { get; set; }
    public void CheckFullyConnected()
    {
        if (inputWireA != null && inputWireB != null && outputWire != null)
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
        switch (id)
        {
            case "Input_A":
                inputWireA = null;

                break;
            case "Input_B":
                inputWireB = null;
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
                inputWireA = wire;
                break;
            case "Input_B":
                inputWireB = wire;
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

    public void UpdateLogic()
    {
        UpdateSprite();
        if (FullyConnected)
        {
            bool newSignal = inputWireA.signal || inputWireB.signal;
            if (newSignal != outputWire.signal)
            {
                outputWire.UpdateSignal(newSignal, this);
            }
        }
        else if (outputWire != null && outputWire.signal)
        {
            outputWire.UpdateSignal(false, this);
        }
    }

    private void UpdateSprite()
    {
        Debug.Log("sprite update");
        string stateA, stateB;

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
        if (inputWireA != null)
        {
            stateA = inputWireA?.signal == true ? "1" : "0";
        }
        else
        {
            stateA = "s";
        }

        if (inputWireB != null)
        {
            stateB = inputWireB?.signal == true ? "1" : "0";
        }
        else
        {
            stateB = "s";
        }



        switch (stateA+ stateB)
        {
            case "11":
                spriteRenderer.sprite = or_11;
                break;
            case "00":
                spriteRenderer.sprite = or_00;
                break;
            case "10":
                spriteRenderer.sprite = or_10;
                break;
            case "01":
                spriteRenderer.sprite = or_01;
                break;
            case "1s":
                spriteRenderer.sprite = or_1s;
                break;
            case "0s":
                spriteRenderer.sprite = or_0s;
                break;
            case "s1":
                spriteRenderer.sprite = or_s1;
                break;
            case "s0":
                spriteRenderer.sprite = or_s0;
                break;
            case "ss":
                spriteRenderer.sprite = or_ss;
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
