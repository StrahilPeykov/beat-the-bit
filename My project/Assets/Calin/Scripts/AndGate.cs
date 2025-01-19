using System;
using UnityEngine;


public class AndGate : Movable, IGate
{
    public const string gateType = "And";

    Wire inputWireA;
    Wire inputWireB;
    Wire outputWire;

    string id = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");



    // Sprites
    [SerializeField] private Sprite and_11;
    [SerializeField] private Sprite and_00;
    [SerializeField] private Sprite and_10;
    [SerializeField] private Sprite and_01;
    [SerializeField] private Sprite and_1s;
    [SerializeField] private Sprite and_0s;
    [SerializeField] private Sprite and_s1;
    [SerializeField] private Sprite and_s0;
    [SerializeField] private Sprite and_ss;

    [SerializeField] private SpriteRenderer spriteRenderer; // To render the sprite on the GameObject

    public bool FullyConnected { get; set; }

    public void CheckFullyConnected()
    {
        FullyConnected = inputWireA != null && inputWireB != null && outputWire != null;
    }

    public void DeRegisterWire(string id)
    {
        switch (id)
        {
            case "Input_A":
                inputWireA = null;
                Debug.Log("intra");
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
                Debug.Log(this.getId());

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
        string stateA = inputWireA?.signal == true ? "1" : inputWireA?.signal == false ? "0" : "s";
        string stateB = inputWireB?.signal == true ? "1" : inputWireB?.signal == false ? "0" : "s";

        if (FullyConnected)
        {
            
            bool newSignal = inputWireA.signal && inputWireB.signal;
            if (outputWire.signal != newSignal)
            {
                //hello 
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
        //Debug.Log("sprite update");
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
        if (inputWireA != null) {
            stateA = inputWireA?.signal == true ? "1" : "0";
        } else
        {
            stateA = "s";
        }

        if (inputWireB != null) {
            stateB = inputWireB?.signal == true ? "1" : "0";
        } else {
            stateB = "s";
        }
        
        

        switch (stateB + stateA)
        {
            case "11":
                spriteRenderer.sprite = and_11;
                break;
            case "00":
                spriteRenderer.sprite = and_00;
                break;
            case "10":
                spriteRenderer.sprite = and_10;
                break;
            case "01":
                spriteRenderer.sprite = and_01;
                break;
            case "1s":
                spriteRenderer.sprite = and_1s;
                break;
            case "0s":
                spriteRenderer.sprite = and_0s;
                break;
            case "s1":
                spriteRenderer.sprite = and_s1;
                break;
            case "s0":
                spriteRenderer.sprite = and_s0;
                break;
            case "ss":
                spriteRenderer.sprite = and_ss;
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
