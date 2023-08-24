using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoller
{
    private static System.Random random = new System.Random();
    private static List<int> nextValuesForTesting = new List<int>();
    private static bool useNextValue = false;

    public static int Roll(int min, int max) {
        if (useNextValue) {
            int val = nextValuesForTesting[0];
            nextValuesForTesting.RemoveAt(0);
            useNextValue = nextValuesForTesting.Count > 0;
            //Debug.Log("Roll(Debug Value): "+val);
            return val;
        }
        var roll = random.Next(min, max);
        //Debug.Log("Roll(Real): "+ roll);
        return roll;
    }

    public static void SetNextTestValue(int value)
    {
        useNextValue = true;
        nextValuesForTesting.Clear();
        nextValuesForTesting.Add(value);
    }

    public static void AddNextTestValue(int value) {
        useNextValue = true; 
        nextValuesForTesting.Add(value);
    }

    public static int GetSuccess(int dice, int tn) {
        int success = 0;

        var rolls = new List<int>();

        for (int i = 0; i < dice; i++) {
            var roll = Roll(1, 10);
            if (roll >= tn) success++;
            rolls.Add(roll);
        }
        var stringRolls = "<";

        for (int i = 0; i < rolls.Count; i++) {
            stringRolls += rolls[i];

            if (i < rolls.Count -1)
                stringRolls += ",";

        }
        stringRolls += ">";
        Debug.Log("Rolls: "+stringRolls);
        return success;
    }

    public static void ClearTestValues() {
        nextValuesForTesting.Clear();
        Debug.Log("Clear Test Values");
    }

}
