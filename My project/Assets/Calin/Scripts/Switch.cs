using System;
using UnityEngine;

public class Switch : Movable, IGate
{
    public const string gateType = "Input";
    public string varName;
    public Canvas canvas;

    public TMPro.TextMeshProUGUI varNameText;

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
        // Find the Transform first and then get the TextMeshProUGUI component
        varNameText = canvas.transform.Find("Variable Name").GetComponent<TMPro.TextMeshProUGUI>();

        // Check if the component is found
        if (varNameText == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on 'Variable Name'!");
        }
        else
        {
            varName = FormulaManager.requestVariableName();

            varNameText.text = varName;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateColor();
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

    void UpdateColor()
    {
        // if (signal)
        // {
        //     spriteRenderer.color = Color.green;
        // }
        // else
        // {
        //     spriteRenderer.color = Color.red;
        // }
    }

    public void OnMouseUpAsButton()
    {
        signal = !signal;
        UpdateColor();
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

    public void updateVarName(string varName)
    {
        this.varName = varName;
        varNameText.text = varName;
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

// using System;
// using UnityEngine;
// using System.Collections.Generic;

// public class Switch : Movable, IGate
// {
//     Wire outputWire;

//     int cnt = 0;

//     string switchId = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
//     private int cntWire = 0;

//     [SerializeField]
//     private bool signal = false;

//     [SerializeField]
//     private Sprite onSprite; // Sprite when signal is true

//     [SerializeField]
//     private Sprite offSprite; // Sprite when signal is false

//     private SpriteRenderer spriteRenderer;

//     public bool FullyConnected { get; set; }

//     void Awake()
//     {
//         spriteRenderer = GetComponent<SpriteRenderer>();
//         UpdateColor();
//     }

//     public void DeRegisterWire(string id)
//     {
//         switch (id)
//         {
//             case "Output_A":
//                 outputWire = null;
//                 cnt--;
//                 Debug.Log(cnt);
//                 Debug.Log("wtf");
//                 break;
//             default:
//                 throw new Exception("ID not supported");
//         }
//         CheckFullyConnected();
//         UpdateLogic();
//     }

//     public void RegisterWire(string id, Wire wire)
//      {   

//         switch (id)
//         {
//             case "Output_A":
//                 outputWire = wire;
//                 // outputWire.getId = switchId;
//                 cnt++;
//                 Debug.Log(cnt);
//                 Debug.Log("input hello" + outputWire.getId());

//                 break;
//             default:
//                 throw new Exception("ID not supported");
//         }
//         CheckFullyConnected();
//         UpdateLogic();
//     }

//     public void CheckFullyConnected()
//     {
//         FullyConnected = (outputWire != null);
//     }

//     public void UpdateLogic()
//     {

//         if (FullyConnected && outputWire.signal != signal)
//         {
//             outputWire.UpdateSignal(signal, this);
//             Debug.Log("update logic switch");
//         }
//     }

//     void UpdateColor()
//     {
//         // spriteRenderer.color = signal ? Color.green : Color.red;
//     }

//     public void OnMouseUpAsButton()
//     {
//         signal = !signal;
//         UpdateColor();
//         UpdateLogic();

//         // Change the sprite based on the signal state
//         if (signal && onSprite != null)
//         {
//             spriteRenderer.sprite = onSprite;
//         }
//         else if (!signal && offSprite != null)
//         {
//             spriteRenderer.sprite = offSprite;
//         }
//     }
// }

// using System;
// using System.Collections.Generic;
// using UnityEngine;

// public class Switch : Movable, IGate
// {
//     Wire wire;

//     string id = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

//     [SerializeField]
//     private bool signal = false;

//     [SerializeField]
//     private Sprite onSprite; // Sprite when signal is true

//     [SerializeField]
//     private Sprite offSprite; // Sprite when signal is false

//     private SpriteRenderer spriteRenderer;

//     public bool FullyConnected { get; set; }

//     void Awake()
//     {
//         spriteRenderer = GetComponent<SpriteRenderer>();
//         UpdateColor();
//     }

//     public void DeRegisterWire(string id)
//     {
//         string split0 = id.Split(' ')[0];
//         Debug.Log(split0);

//         string split1 = id.Split(' ')[1];
//         switch (split0)
//         {
//             case "Output_A":
//                 if (wires.ContainsKey(split1))
//                 {
//                     wires.Remove(split1);
//                     Debug.Log($"Wire {split1} deregistered.");
//                 }
//                 else
//                 {
//                     Debug.LogWarning($"Wire with ID {split1} not found for deregistration.");
//                 }
//                 break;

//             default:
//                 throw new Exception($"ID {split1} not supported for deregistration.");
//         }

//         CheckFullyConnected();
//         UpdateLogic();
//     }

//     public void RegisterWire(string id, Wire wire)
//     {
//         switch (id)
//         {
//             case "Output_A":
//                 if (wires.ContainsKey(wire.getId()))
//                 {
//                     Debug.LogWarning($"Wire with ID {wire.getId()} is already registered.");
//                 }
//                 else
//                 {
//                     wires[wire.getId()] = wire;
//                 }
//                 break;

//             default:
//                 throw new Exception($"ID {id} not supported for registration.");
//         }

//         CheckFullyConnected();
//         UpdateLogic();
//     }

//     public void CheckFullyConnected()
//     {
//         // FullyConnected is true if there is at least one wire registered
//         FullyConnected = wires.Count > 0;
//     }

//     public void UpdateLogic()
//     {
//         if (FullyConnected)
//         {
//             foreach (var wire in wires.Values)
//             {
//                 if (wire.signal != signal)
//                 {
//                     wire.UpdateSignal(signal, this);
//                     Debug.Log($"Updated signal for wire with ID {wire.getId()}.");
//                 }
//             }
//         }
//     }

//     void UpdateColor()
//     {
//         // Change color based on signal
//         if (signal)
//         {
//             spriteRenderer.color = Color.green;
//         }
//         else
//         {
//             spriteRenderer.color = Color.red;
//         }
//     }

//     public void OnMouseUpAsButton()
//     {
//         // Toggle the signal and update the visual state
//         signal = !signal;
//         UpdateColor();
//         UpdateLogic();

//         // Change the sprite based on the signal state
//         if (signal && onSprite != null)
//         {
//             spriteRenderer.sprite = onSprite;
//         }
//         else if (!signal && offSprite != null)
//         {
//             spriteRenderer.sprite = offSprite;
//         }
//     }

//     public string getId()
//     {
//         return this.id;
//     }
// }
