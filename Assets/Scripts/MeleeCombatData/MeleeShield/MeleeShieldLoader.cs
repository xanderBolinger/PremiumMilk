using System.Collections.Generic;
using UnityEngine;

public class MeleeShieldLoader : MonoBehaviour
{
    public static MeleeShield GetShieldByName(string name)
    {
        var shields = ReadShields();

        foreach (var shield in shields)
        {
            if (shield.shieldName == name)
                return shield;
        }

        throw new System.Exception("Shield not found for name: " + name);
    }

    public static List<string> GetShieldNames() {
        List<string> shieldList = new List<string>();

        string fileName = "ShieldData";
        TextAsset shieldData = Resources.Load<TextAsset>(fileName);
        string csvText = shieldData.text;

        // Split the CSV selectedItemName into individual lines
        string[] csvLines = csvText.Split('\n');

        // Process each line (skip the header line)
        for (int i = 1; i < csvLines.Length; i++)
        {
            string line = csvLines[i].Trim();

            // Split the line into individual values
            string[] values = line.Split(',');

            // Extract the values for creating a MeleeShield instance
            string shieldName = values[0];
            shieldList.Add(shieldName);
        }

        return shieldList;
    }

    public static List<MeleeShield> ReadShields()
    {
        List<MeleeShield> shieldList = new List<MeleeShield>();

        string fileName = "ShieldData";
        TextAsset shieldData = Resources.Load<TextAsset>(fileName);
        string csvText = shieldData.text;

        // Split the CSV selectedItemName into individual lines
        string[] csvLines = csvText.Split('\n');

        // Process each line (skip the header line)
        for (int i = 1; i < csvLines.Length; i++)
        {
            string line = csvLines[i].Trim();

            // Split the line into individual values
            string[] values = line.Split(',');

            // Extract the values for creating a MeleeShield instance
            string shieldName = values[0];
            int DTN = int.Parse(values[1]);

            // Create the MeleeShield instance
            MeleeShield shield = new MeleeShield
            {
                shieldName = shieldName,
                DTN = DTN
            };

            // Add the shield to the list
            shieldList.Add(shield);
        }

        return shieldList;
    }
}
