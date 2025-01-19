using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;

public class FormulaManager : MonoBehaviour
{
    public TextUpdater textUpdater;
    public static List<Switch> inputList = new List<Switch>();

    // public static List<string> assignedVarNames = new List<string>();
    public static Probe probe;

    public static int varCnt = -1;

    public static int probeCnt = 0;
    public static string[] alphabet = new string[]
{
    "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
};


    // GameObject textField;

    public TMPro.TextMeshProUGUI text;

    public static Tuple<string, int, Dictionary<string, int>> formula;

    void Start()
    {
        formula = FormulaGenerator.GenerateFormula();
        if (text == null)
        {
            Debug.LogWarning("nu e text field");
        }
        else
        {
            // text.text = "To make: " + formula.Item1;
            textUpdater.formulaText.text = "To make: " + formula.Item1;
        }
        textUpdater.formulaText.text = "To make: " + formula.Item1;
    }

    public static string getNewFormula()
    {
        formula = FormulaGenerator.GenerateFormula();
        return formula.Item1;
    }

    public static void assignInput(Switch newSwitch)
    {
        // register input in list
        inputList.Add(newSwitch);
        Debug.Log(newSwitch.varName);
    }

    public static void removeInput(Switch deletedSwitch)
    {
        // char removedName = deletedSwitch.varName[0];
        // int removedCode = (int) removedName;
        int removedCode = (int)deletedSwitch.varName[0];

        inputList.Remove(deletedSwitch);

        foreach (Switch input in inputList)
        {
            // char aux = input.varName[0];

            // int auxCode = (int) aux;
            int auxCode = (int)input.varName[0];

            if (auxCode >= removedCode)
            {
                auxCode--;
            }

            input.updateVarName(((char)auxCode).ToString());
        }


        giveBackVarName();
        Debug.Log(inputList.Count);



    }

    // called in Switch class!
    public static string requestVariableName()
    {
        varCnt++;
        // assignedVarNames.Add(alphabet[varCnt]);
        return alphabet[varCnt];
    }

    public static void giveBackVarName()
    {
        varCnt--;
    }

    // method to update the variable names of input gates such that they are alphabetically consecutively
    // private static void updateVarName() {
    //     // nothing for now
    // }

    public static int getProbeCount()
    {
        return probeCnt;
    }

    public static void assignProbe(Probe newProbe)
    {
        probe = newProbe;
        Debug.Log(probe.getId());
        probeCnt++;
    }
    public static void removeProbe()
    {
        probe = null;
        probeCnt--;
    }
}
