using System;
using System.Collections.Generic;

public class FormulaGenerator
{
    private static int cnt = 0;
    private static List<Tuple<string, int, Dictionary<string, int>>> formulas = new List<Tuple<string, int, Dictionary<string, int>>>
    {
        // Formula 1: (A && B)
        new Tuple<string, int, Dictionary<string, int>>(
            "(A && B)",
            2,
            new Dictionary<string, int>
            {
                { "00", 0 },
                { "01", 0 },
                { "10", 0 },
                { "11", 1 }
            }),

        // Formula 2: (A || B)
        new Tuple<string, int, Dictionary<string, int>>(
            "(A || B)",
            2,
            new Dictionary<string, int>
            {
                { "00", 0 },
                { "01", 1 },
                { "10", 1 },
                { "11", 1 }
            }),

        // Formula 4: !(A && B)
        new Tuple<string, int, Dictionary<string, int>>(
            "!(A && B)",
            2,
            new Dictionary<string, int>
            {
                { "00", 1 },
                { "01", 1 },
                { "10", 1 },
                { "11", 0 }
            }),

        // Formula 5: !(A || B)
        new Tuple<string, int, Dictionary<string, int>>(
            "!(A || B)",
            2,
            new Dictionary<string, int>
            {
                { "00", 1 },
                { "01", 0 },
                { "10", 0 },
                { "11", 0 }
            }),

        // Formula 3: ((A && B) || C)
        new Tuple<string, int, Dictionary<string, int>>(
            "((A && B) || C)",
            3,
            new Dictionary<string, int>
            {
                { "000", 0 },
                { "001", 1 },
                { "010", 0 },
                { "011", 1 },
                { "100", 0 },
                { "101", 1 },
                { "110", 1 },
                { "111", 1 }
            }),    

        // Formula 6: A && (B || C)
        new Tuple<string, int, Dictionary<string, int>>(
            "A && (B || C)",
            3,
            new Dictionary<string, int>
            {
                { "000", 0 },
                { "001", 0 },
                { "010", 0 },
                { "011", 0 },
                { "100", 0 },
                { "101", 1 },
                { "110", 1 },
                { "111", 1 }
            }),

        // Formula 7: A || (B && C)
        new Tuple<string, int, Dictionary<string, int>>(
            "A || (B && C)",
            3,
            new Dictionary<string, int>
            {
                { "000", 0 },
                { "001", 0 },
                { "010", 0 },
                { "011", 1 },
                { "100", 1 },
                { "101", 1 },
                { "110", 1 },
                { "111", 1 }
            }),

        // Formula 8: ((A && B) && C)
        new Tuple<string, int, Dictionary<string, int>>(
            "((A && B) && C)",
            3,
            new Dictionary<string, int>
            {
                { "000", 0 },
                { "001", 0 },
                { "010", 0 },
                { "011", 0 },
                { "100", 0 },
                { "101", 0 },
                { "110", 0 },
                { "111", 1 }
            }),

        // Formula 9: A || (B || C)
        new Tuple<string, int, Dictionary<string, int>>(
            "A || (B || C)",
            3,
            new Dictionary<string, int>
            {
                { "000", 0 },
                { "001", 1 },
                { "010", 1 },
                { "011", 1 },
                { "100", 1 },
                { "101", 1 },
                { "110", 1 },
                { "111", 1 }
            }),

        // Formula 10: ((A && B) && (C && D))
        new Tuple<string, int, Dictionary<string, int>>(
            "((A && B) && (C && D))",
            4,
            new Dictionary<string, int>
            {
                { "0000", 0 },
                { "0001", 0 },
                { "0010", 0 },
                { "0011", 0 },
                { "0100", 0 },
                { "0101", 0 },
                { "0110", 0 },
                { "0111", 0 },
                { "1000", 0 },
                { "1001", 0 },
                { "1010", 0 },
                { "1011", 0 },
                { "1100", 0 },
                { "1101", 0 },
                { "1110", 0 },
                { "1111", 1 }
            }),

        // Formula 11: (A || (B && C)) || D
        new Tuple<string, int, Dictionary<string, int>>(
            "(A || (B && C)) || D",
            4,
            new Dictionary<string, int>
            {
                { "0000", 0 },
                { "0001", 1 },
                { "0010", 0 },
                { "0011", 1 },
                { "0100", 0 },
                { "0101", 1 },
                { "0110", 1 },
                { "0111", 1 },
                { "1000", 1 },
                { "1001", 1 },
                { "1010", 1 },
                { "1011", 1 },
                { "1100", 1 },
                { "1101", 1 },
                { "1110", 1 },
                { "1111", 1 }
            }),

        // Formula 12: A && !(B || C)
        new Tuple<string, int, Dictionary<string, int>>(
            "A && !(B || C)",
            3,
            new Dictionary<string, int>
            {
                { "000", 0 },
                { "001", 0 },
                { "010", 0 },
                { "011", 0 },
                { "100", 1 },
                { "101", 0 },
                { "110", 0 },
                { "111", 0 }
            }),

        // Formula 13: !(A || (B && C))
        new Tuple<string, int, Dictionary<string, int>>(
            "!(A || (B && C))",
            3,
            new Dictionary<string, int>
            {
                { "000", 1 },
                { "001", 1 },
                { "010", 1 },
                { "011", 0 },
                { "100", 0 },
                { "101", 0 },
                { "110", 0 },
                { "111", 0 }
            }),

        // Formula 14: ((A && B) || (C && D))
        new Tuple<string, int, Dictionary<string, int>>(
            "((A && B) || (C && D))",
            4,
            new Dictionary<string, int>
            {
                { "0000", 0 },
                { "0001", 0 },
                { "0010", 0 },
                { "0011", 1 },
                { "0100", 0 },
                { "0101", 0 },
                { "0110", 0 },
                { "0111", 1 },
                { "1000", 0 },
                { "1001", 0 },
                { "1010", 0 },
                { "1011", 1 },
                { "1100", 1 },
                { "1101", 1 },
                { "1110", 1 },
                { "1111", 1 }
            })
    };

    private static Random rng = new Random();

    // Method to pick a random formula from the list
    public static Tuple<string, int, Dictionary<string, int>> GenerateFormula()
    {
        int randomIndex;
        
        switch (cnt) {
            case 0:
                randomIndex = rng.Next(4);
                break;
            
            case 1:
                randomIndex = rng.Next(4, 9);
                break;
            
            case 2:
                randomIndex = rng.Next(4, 9);
                break;
            
            case 3:
                randomIndex = rng.Next(4, 9);
                break;

            default:
                randomIndex = rng.Next(9, formulas.Count);
                break;
        }

        cnt++;
        
        return formulas[randomIndex];
    }
}
