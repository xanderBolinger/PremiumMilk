using System;
using System.Collections.Generic;
using UnityEngine;

public class ArmorLoader : MonoBehaviour
{

    public static ArmorPiece GetArmorPieceByName(string name) {
        var armor = ReadArmor();

        foreach (var piece in armor) {
            if (piece.Name == name)
                return piece;
        }

        throw new System.Exception("Armor piece not found for name: "+name);
    }

    public static List<ArmorPiece> ReadArmor()
    {
        List<ArmorPiece> armorList = new List<ArmorPiece>();

        string fileName = "ArmorData";
        TextAsset armorData = Resources.Load<TextAsset>(fileName);
        string csvText = armorData.text;

        // Split the CSV selectedItemName into individual lines
        string[] csvLines = csvText.Split('\n');

        // Process each line (skip the header line)
        for (int i = 1; i < csvLines.Length; i++)
        {
            string line = csvLines[i].Trim();

            // Split the line into individual values
            string[] values = line.Split(',');

            // Extract the values for creating an ArmorPiece instance
            string name = values[0];
            int armorValue = int.Parse(values[1]);
            int weight = int.Parse(values[2]);
            ArmorPiece.ArmorMaterial material = (ArmorPiece.ArmorMaterial)System.Enum.Parse(typeof(ArmorPiece.ArmorMaterial), values[3]);
            string[] slotValues = values[4].Split(';');
            List<ArmorPiece.ArmorPieceSlots> slots = new List<ArmorPiece.ArmorPieceSlots>();

            foreach (string slotValue in slotValues)
            {
                ArmorPiece.ArmorPieceSlots slot = (ArmorPiece.ArmorPieceSlots)System.Enum.Parse(typeof(ArmorPiece.ArmorPieceSlots), slotValue);
                slots.Add(slot);
            }

            // Create the ArmorPiece instance
            ArmorPiece armorPiece = new ArmorPiece(name, armorValue, weight, material, slots);

            // Add the armor piece to the list
            armorList.Add(armorPiece);
        }

        return armorList;
    }

    public static List<string> GetArmorNames()
    {
        List<string> armorNames = new List<string>();

        string fileName = "ArmorData";
        TextAsset armorData = Resources.Load<TextAsset>(fileName);
        string csvText = armorData.text;

        // Split the CSV selectedItemName into individual lines
        string[] csvLines = csvText.Split('\n');

        // Process each line (skip the header line)
        for (int i = 1; i < csvLines.Length; i++)
        {
            string line = csvLines[i].Trim();

            // Split the line into individual values
            string[] values = line.Split(',');

            // Extract the values for creating an ArmorPiece instance
            string name = values[0];
            armorNames.Add(name);
        }


        return armorNames;
    }
}
