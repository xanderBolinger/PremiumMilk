using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ExcelUtillity {
    public class MeleeHitLocation
    {
        // Files stuff
        private static string path = Application.dataPath;

        public enum MeleeDamageType
        {
            CUTTING, PIERICNG, BLUNT
        }

        public static string GetHitLocationZoneName(MeleeDamageType damageType, int zone, int subZoneRoll) {
            var location = GetMeleeHitLocation(damageType, 1, zone, subZoneRoll);
            return location.zoneName;
        }

        public static string GetAnatomicalHitLocation(MeleeDamageType damageType, string zoneName) {

            string fileName = GetFileName(damageType);
            var (pdText, pcHitLocation) = GetPDText(0, 10, zoneName, fileName, damageType);
            return pcHitLocation;
        }

        public static (int, bool, string) GetMeleeHitPD(MeleeDamageType damageType, int originalDamagePoints, int AV, string zoneName)
        {
            string fileName = GetFileName(damageType);

            AV = 0;

            var (pdText, pcHitLocation) = GetPDText(AV, originalDamagePoints, zoneName, fileName, damageType);
            var (physicalDamage, disabled) = GetPdFromCell(pdText);
            return (physicalDamage, disabled, pcHitLocation);
        }

        private static string GetFileName(MeleeDamageType damageType) {
            string fileName;

            switch (damageType)
            {
                case MeleeDamageType.CUTTING:
                    fileName = "PcDamageTableCutting";
                    break;
                case MeleeDamageType.PIERICNG:
                    fileName = "PcDamageTableStabbing";
                    break;
                case MeleeDamageType.BLUNT:
                    fileName = "PcDamageTableBlunt";
                    break;
                default:
                    throw new Exception("Damage Type not found for damage type of: " + damageType);
            }

            return fileName;
        }

        public static MeleeHitLocationData GetMeleeHitLocation(MeleeDamageType damageType, int damageLevel, int zone, int subZoneRoll)
        {
            string fileName;

            switch (damageType)
            {
                case MeleeDamageType.CUTTING:
                    fileName = "MeleeDamageTablesCutting";
                    break;
                case MeleeDamageType.PIERICNG:
                    fileName = "MeleeDamageTablesPuncture";
                    break;
                case MeleeDamageType.BLUNT:
                    fileName = "MeleeDamageTablesBlunt";
                    break;
                default:
                    throw new Exception("Damage Type not found for damage type of: " + damageType);
            }

            return GetLocationText(fileName, zone, damageLevel, subZoneRoll);
        
        }

        private static MeleeHitLocationData GetLocationText(string fileName, int zone, int damageLevel, int subZoneRoll)
        {
            string zoneName = "";
            int bloodLossPD = 0;
            int shockPD = 0;
            int painPoints = 0;
            bool knockDown = false;
            int knockDownMod = 0;

            TextAsset textAsset = Resources.Load<TextAsset>("MeleeHitTables/" + fileName);
            string[] lines = textAsset.text.Split('\n');

            foreach (string line in lines)
            {
                string[] row = line.Split(',');

                bool zoneFound = false;
                if (int.TryParse(row[0], out _))
                {
                    if (zone == int.Parse(row[0]))
                    {
                        zoneFound = true;
                    }
                }

                if (zoneFound)
                {
                    string[] paddingRow = lines[Array.IndexOf(lines, line) + 1].Split(',');

                    // Loops through sub zones 
                    for (int i = Array.IndexOf(lines, line) + 2; i < lines.Length; i++)
                    {
                        string[] subZoneRow = lines[i].Split(',');
                        bool foundSubZone = false;

                        // Gets sub zone values 
                        // Stores them in a list 
                        string zones = subZoneRow[1];
                        List<string> zoneNumbers = zones.Split(';').ToList();
                        if (zoneNumbers.Contains(subZoneRoll.ToString()))
                        {
                            zoneName = subZoneRow[2];
                            //Format: BL,S,P,KD(Y / N),KDM
                            string zoneDamageLevel = subZoneRow[2 + damageLevel];
                            List<string> zoneDamageLevelValues = zoneDamageLevel.Split(';').ToList();

                            bloodLossPD = int.Parse(zoneDamageLevelValues[0]);
                            shockPD = int.Parse(zoneDamageLevelValues[1]);
                            painPoints = int.Parse(zoneDamageLevelValues[2]);

                            string knockDownValue = zoneDamageLevelValues[3];

                            knockDown = knockDownValue == "Y";
                            knockDownMod = int.Parse(zoneDamageLevelValues[4]);

                            foundSubZone = true;
                        }

                        if (foundSubZone)
                            break;
                    }

                    break;
                }
            }

            return new MeleeHitLocationData(zoneName, bloodLossPD, shockPD, painPoints, knockDown, knockDownMod);
        }

        /*private static MeleeHitLocationData GetLocationText(string fileName, int zone, int damageLevel, int subZoneRoll)
        {
            string zoneName = "";
            int bloodLossPD = 0;
            int shockPD = 0;
            int painPoints = 0;
            bool knockDown = false;
            int knockDownMod = 0;
            
            //TextAsset textAsset = Resources.Load<TextAsset>(@"MeleeHitTables\"+fileName);
            //var p = new TextFieldParser();
            using (TextFieldParser parser = new TextFieldParser(path + @"\CSVs\MeleeHitTables\" + fileName))
            {
                parser.Delimiters = new string[] { "," };

                while (true)
                {
                    string[] row = parser.ReadFields();
                    //Debug.Log("Zone: " + row[0]);
                    bool zoneFound = false;
                    if (int.TryParse(row[0], out _))
                    {

                        if (zone == Int32.Parse(row[0]))
                        {

                            zoneFound = true;
                        }

                    }


                    if (zoneFound)
                    {

                        string[] paddingRow = parser.ReadFields();

                        // Loops through sub zones 
                        while (true)
                        {
                            string[] subZoneRow = parser.ReadFields();
                            bool foundSubZone = false;

                            // Gets sub zone values 
                            // Stores them in a list 
                            string zones = subZoneRow[1];
                            List<string> zoneNumbers = zones.Split(',').ToList();
                            if (zoneNumbers.Contains(subZoneRoll.ToString()))
                            {
                                zoneName = subZoneRow[2];
                                string zoneDamageLevel = subZoneRow[2 + damageLevel];
                                List<string> zomeDamageLevelValues = zoneDamageLevel.Split(',').ToList();

                                bloodLossPD = Int32.Parse(zomeDamageLevelValues[0]);
                                shockPD = Int32.Parse(zomeDamageLevelValues[1]);
                                painPoints = Int32.Parse(zomeDamageLevelValues[2]);

                                string knockDownValue = zomeDamageLevelValues[3];

                                if (knockDownValue == "Y")
                                    knockDown = true;
                                else
                                    knockDown = false;

                                knockDownMod = Int32.Parse(zomeDamageLevelValues[4]);

                                foundSubZone = true;

                            }

                            if (foundSubZone)
                                break;
                        }

                        break;
                    }
                }

            }

            return new MeleeHitLocationData(zoneName, bloodLossPD, shockPD, painPoints, knockDown, knockDownMod);
        }*/


        /*private static (string, string) GetPDText(int armorValue, int damagePoints, string hitLocation, string fileName, MeleeDamageType damageType)
        {
            int avRow = GetAvRow(armorValue, fileName);
            int damageColumn = GetDamageColumn(avRow, damagePoints, fileName);
            int count = 0;

            using (TextFieldParser parser = new TextFieldParser(path + @"\CSVs\MeleeHitTables\" + fileName))
            {
                parser.Delimiters = new string[] { "," };

                while (true)
                {
                    string[] row = parser.ReadFields();

                    if (row == null)
                        throw new Exception("Hit Location Table for: "+fileName+", With AV: "+armorValue+" and Damage: "
                            +damagePoints+" searching for Hit Location: "+hitLocation+" not found.");

                    string cellValue = row[0];


                    switch (damageType)
                    {
                        case MeleeDamageType.CUTTING:
                            var (returnFlag, cuttingCellVale, pcHitLocation) = GetCuttingCellValue(fileName, damageColumn, hitLocation);
                            if (returnFlag)
                                return (cuttingCellVale, pcHitLocation);
                            break;
                        case MeleeDamageType.PEIRICNG:
                            var (returnFlagStabbing, stabbingCellVale, pcHitLocation1)
                                = GetStabbingCellValue(fileName, damageColumn, hitLocation);
                            if (returnFlagStabbing)
                                return (stabbingCellVale, pcHitLocation1);
                            break;
                        case MeleeDamageType.BLUNT:
                            var (returnFlagBlunt, bluntCellValue, pcHitLocation2)
                                = GetBluntCellValue(fileName, damageColumn, hitLocation);
                            if (returnFlagBlunt)
                                return (bluntCellValue, pcHitLocation2);
                            break;
                    }




                    if (cellValue == hitLocation)
                    {
                        return (row[damageColumn], hitLocation);
                    }



                    count++;

                }

            }

        }*/


    private static (string, string) GetPDText(int armorValue, int damagePoints, string hitLocation, string fileName, MeleeDamageType damageType)
    {
        int avRow = GetAvRow(armorValue, fileName);
        int damageColumn = GetDamageColumn(avRow, damagePoints, fileName);
        int count = 0;

        TextAsset textAsset = Resources.Load<TextAsset>("MeleeHitTables/" + fileName);
        string[] lines = textAsset.text.Split('\n');

        foreach (string line in lines)
        {
            string[] row = line.Split(',');

            if (row == null)
            {
                throw new Exception("Hit Location Table for: " + fileName + ", With AV: " + armorValue + " and Damage: " +
                    damagePoints + " searching for Hit Location: " + hitLocation + " not found.");
            }

            string cellValue = row[0];

            switch (damageType)
            {
                case MeleeDamageType.CUTTING:
                    (bool returnFlag, string cuttingCellValue, string pcHitLocation) = GetCuttingCellValue(fileName, damageColumn, hitLocation);
                    if (returnFlag)
                    {
                        return (cuttingCellValue, pcHitLocation);
                    }
                    break;
                case MeleeDamageType.PIERICNG:
                    (bool returnFlagStabbing, string stabbingCellValue, string pcHitLocation1) = GetStabbingCellValue(fileName, damageColumn, hitLocation);
                    if (returnFlagStabbing)
                    {
                        return (stabbingCellValue, pcHitLocation1);
                    }
                    break;
                case MeleeDamageType.BLUNT:
                    (bool returnFlagBlunt, string bluntCellValue, string pcHitLocation2) = GetBluntCellValue(fileName, damageColumn, hitLocation);
                    if (returnFlagBlunt)
                    {
                        return (bluntCellValue, pcHitLocation2);
                    }
                    break;
            }

            if (cellValue == hitLocation)
            {
                return (row[damageColumn], hitLocation);
            }

            count++;
        }

        throw new Exception("Hit Location Table for: " + fileName + ", With AV: " + armorValue + " and Damage: " +
            damagePoints + " searching for Hit Location: " + hitLocation + " not found.");
    }


    private static (bool, string, string) GetBluntCellValue(string fileName, int damageColumn,
            string hitLocation)
        {

            if ("Neck" == hitLocation) {
                return (true, GetCellValue(9, damageColumn, fileName), "Throat");
            }

            if ("Forearm" == hitLocation || "Elbow" == hitLocation)
            {
                int roll = DiceRoller.Roll(1, 15);

                if (roll <= 9)
                {
                    // Upper arm 
                    return (true, GetCellValue(15, damageColumn, fileName), "Upper Arm");
                }
                else
                {
                    // Forearm 
                    return (true, GetCellValue(16, damageColumn, fileName), "Forearm");
                }

            }

            if ("Chest" == hitLocation || "Ribcage" == hitLocation)
            {
                int roll = DiceRoller.Roll(1, 3);

                if (roll == 1)
                {
                    // Lower chest 
                    return (true, GetCellValue(12, damageColumn, fileName), "Lower Chest");
                }
                else
                {
                    // Upper chest 
                    return (true, GetCellValue(11, damageColumn, fileName), "Upper Chest");
                }
            }
            else if ("Hip" == hitLocation)
                return (true, GetCellValue(14, damageColumn, fileName), "Pelvis");

            return (false, "", "");
        }

        private static (bool, string, string) GetCuttingCellValue(string fileName, int damageColumn,
            string hitLocation)
        {
            if ("Neck" == hitLocation)
            {
                return (true, GetCellValue(9, damageColumn, fileName), "Throat");
            }

            if ("Forearm" == hitLocation || "Elbow" == hitLocation)
            {
                int roll = DiceRoller.Roll(1, 15);

                if (roll <= 9)
                {
                    // Upper arm 
                    return (true, GetCellValue(15, damageColumn, fileName), "Upper Arm");
                }
                else
                {
                    // Forearm 
                    return (true, GetCellValue(16, damageColumn, fileName), "Forearm");
                }

            }

            if ("Chest" == hitLocation || "Ribcage" == hitLocation)
            {
                int roll = DiceRoller.Roll(1, 3);

                if (roll == 1)
                {
                    // Lower chest 
                    return (true, GetCellValue(12, damageColumn, fileName), "Lower Chest");
                }
                else
                {
                    // Upper chest 
                    return (true, GetCellValue(11, damageColumn, fileName),"Upper Chest");
                }
            }

            else if ("Hip" == hitLocation)
                return (true, GetCellValue(14, damageColumn, fileName), "Pelvis");

            return (false, "", "");
        }

        private static (bool, string, string) GetStabbingCellValue(string fileName, int damageColumn,
            string hitLocation)
        {


            if ("Shoulder" == hitLocation)
            {
                int roll = DiceRoller.Roll(20, 25);

                if (roll <= 21)
                {
                    // Shoulder socket
                    return (true, GetCellValue(12, damageColumn, fileName), "Shoulder Socket");
                }
                else
                {
                    // Shoulder scapula 
                    return (true, GetCellValue(13, damageColumn, fileName), "Shoulder Scapula");
                }

            }

            else if ("Skull" == hitLocation)
            {
                return (true, GetCellValue(7, damageColumn, fileName), "Skull");
            }

            else if ("Face" == hitLocation)
            {
                int roll = DiceRoller.Roll(6, 14);

                if (roll <= 7)
                {
                    // Eye 
                    return (true, GetCellValue(8, damageColumn, fileName), "Eye");
                }
                else
                {
                    // Mouth
                    return (true, GetCellValue(9, damageColumn, fileName), "Mouth");
                }
            }

            else if ("Throat" == hitLocation)
            {
                int roll = DiceRoller.Roll(15, 19);

                if (roll <= 17)
                {
                    // Neck
                    return (true, GetCellValue(10, damageColumn, fileName), "Neck");
                }
                else
                {
                    // Base of neck 
                    return (true, GetCellValue(11, damageColumn, fileName), "Base of Neck");
                }
            }

            else if ("Chest" == hitLocation || "Ribcage" == hitLocation)
            {
                int roll = DiceRoller.Roll(26, 39);

                if (roll <= 30)
                {
                    // Lung
                    return (true, GetCellValue(14, damageColumn, fileName), "Lung");
                }
                else if (roll <= 32)
                {
                    // Heart
                    return (true, GetCellValue(15, damageColumn, fileName), "Heart");
                }
                else if (roll <= 34)
                {
                    // Liver
                    return (true, GetCellValue(16, damageColumn, fileName), "Liver");
                }
                else if (roll <= 36)
                {
                    // Liver-Spine
                    return (true, GetCellValue(19, damageColumn, fileName), "Liver-Spine");
                }
                else if (roll <= 38)
                {
                    // Liver-Kidney
                    return (true, GetCellValue(20, damageColumn, fileName), "Liver-Kidney");
                }
                else
                {
                    // Spine 
                    return (true, GetCellValue(22, damageColumn, fileName), "Spine");
                }
            }

            else if ("Stomach" == hitLocation)
            {
                int roll = DiceRoller.Roll(39, 54);
                if (roll <= 39)
                {
                    // Stomach 
                    return (true, GetCellValue(17, damageColumn, fileName), "Stomach");
                }
                if (roll <= 43)
                {
                    // Stomach Kidney
                    return (true, GetCellValue(18, damageColumn, fileName), "Stomach Kidney");
                }
                if (roll <= 46)
                {
                    // Intestines
                    return (true, GetCellValue(21, damageColumn, fileName), "Instestines");
                }
                if (roll <= 47)
                {
                    // Spine  
                    return (true, GetCellValue(22, damageColumn, fileName), "Spine");
                }
                if (roll <= 54)
                {
                    // Intestines-Pelvis
                    return (true, GetCellValue(23, damageColumn, fileName), "Intestines-Pelvis");
                }

            }
            else if ("Pelvis" == hitLocation) {
                int roll = DiceRoller.Roll(1, 3);
                if (roll == 1)
                {
                    return (true, GetCellValue(21, damageColumn, fileName), "Instestines");
                }
                else if (roll == 2)
                {
                    return (true, GetCellValue(23, damageColumn, fileName), "Intestines-Pelvis");
                }
                else {
                    return (true, GetCellValue(24, damageColumn, fileName), "Hip Socket");
                }
            }
            else if ("Forearm" == hitLocation || "Elbow" == hitLocation)
            {
                int roll = DiceRoller.Roll(59, 74);

                if (roll <= 66)
                {
                    // Upper arm 
                    return (true, GetCellValue(25, damageColumn, fileName), "Upper Arm");
                }
                else
                {
                    // Forearm 
                    return (true, GetCellValue(26, damageColumn, fileName), "Forearm");
                }

            }

            else if ("Hip" == hitLocation)
                return (true, GetCellValue(24, damageColumn, fileName), "Hip Socket");

            return (false, "", "");
        }


    private static string GetCellValue(int rowIndex, int columnIndex, string fileName)
    {
        TextAsset textAsset = Resources.Load<TextAsset>("MeleeHitTables/" + fileName);
        string[] lines = textAsset.text.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            string[] row = lines[i].Split(',');

            if (i == rowIndex)
            {
                return row[columnIndex];
            }
        }

        return null;
    }


        /*private static string GetCellValue(int rowIndex, int columnIndex, string fileName)
        {

            int count = 0;

            using (TextFieldParser parser = new TextFieldParser(path + @"\CSVs\MeleeHitTables\" + fileName))
            {
                parser.Delimiters = new string[] { "," };

                while (true)
                {
                    string[] row = parser.ReadFields();


                    if (count == rowIndex)
                    {

                        return row[columnIndex];

                    }

                    count++;

                }

            }



        }*/

        /*private static int getAvRow(int armorValue, string fileName)
        {

            int count = 0;

            using (TextFieldParser parser = new TextFieldParser(path + @"\CSVs\MeleeHitTables\" + fileName))
            {
                parser.Delimiters = new string[] { "," };

                while (true)
                {
                    string[] row = parser.ReadFields();

                    if (int.TryParse(row[0], out _))
                    {
                        int cellArmorValue = Int32.Parse(row[0]);
                        if (armorValue >= cellArmorValue)
                        {
                            return count;
                        }

                    }

                    count++;

                }

            }
        }

        private static int GetDamageColumn(int avRow, int damagePoints, string fileName)
        {
            int count = 0;

            using (TextFieldParser parser = new TextFieldParser(path + @"\CSVs\MeleeHitTables\" + fileName))
            {
                parser.Delimiters = new string[] { "," };

                while (true)
                {
                    string[] row = parser.ReadFields();
                    if (count == avRow)
                    {
                        //Debug.Log("AV Row: " + avRow);
                        for (int i = 27; i >= 1; i--)
                        {

                            if (int.TryParse(row[i], out _))
                            {

                                int damageValue = Int32.Parse(row[i]);
                                if (damagePoints >= damageValue)
                                {
                                    return i;
                                }

                            }



                            if (i == 1)
                            {
                                return 1;
                            }
                        }
                    }

                    count++;

                }

            }
        }*/

    private static int GetAvRow(int armorValue, string fileName)
    {
        TextAsset textAsset = Resources.Load<TextAsset>("MeleeHitTables/" + fileName);
        string[] lines = textAsset.text.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            string[] row = lines[i].Split(',');

            if (int.TryParse(row[0], out _))
            {
                int cellArmorValue = int.Parse(row[0]);
                if (armorValue >= cellArmorValue)
                {
                    return i;
                }
            }
        }

        return -1; // Indicate that the armor value is not found
    }

    private static int GetDamageColumn(int avRow, int damagePoints, string fileName)
    {
        TextAsset textAsset = Resources.Load<TextAsset>("MeleeHitTables/" + fileName);
        string[] lines = textAsset.text.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            string[] row = lines[i].Split(',');

            if (i == avRow)
            {
                for (int j = 27; j >= 1; j--)
                {
                    if (int.TryParse(row[j], out _))
                    {
                        int damageValue = int.Parse(row[j]);
                        if (damagePoints >= damageValue)
                        {
                            return j;
                        }
                    }

                    if (j == 1)
                    {
                        return 1;
                    }
                }
            }
        }

        return -1; // Indicate that the damage column is not found
    }


        private static (int, bool) GetPdFromCell(string damageCell)
    {
        int physicalDamage = 0;
        bool disabled = false;

        if (damageCell.Length == 1)
        {
            //System.out.println("CharAt1: "+damageCell.charAt(0));
            physicalDamage = Int32.Parse(damageCell);
        }
        else if (damageCell.Length == 2)
        {
            //System.out.println("CharAt1: "+damageCell.charAt(0));
            //System.out.println("CharAt2: "+damageCell.charAt(1));
            if (Char.IsDigit(damageCell[1]))
            {
                physicalDamage = Int32.Parse(damageCell);
            }
            else
            {
                if (damageCell[1] == 'D')
                {
                    disabled = true;

                    physicalDamage = (int)Char.GetNumericValue(damageCell[0]);
                }
                else
                {
                    if (damageCell[1] == 'H')
                    {
                        physicalDamage = (int)Char.GetNumericValue(damageCell[0]) * 100;
                    }
                    else if (damageCell[1] == 'K')
                    {
                        physicalDamage = (int)Char.GetNumericValue(damageCell[0]) * 1000;
                    }
                    else if (damageCell[1] == 'T')
                    {
                        physicalDamage = (int)Char.GetNumericValue(damageCell[0]) * 10000;
                    }
                    else if (damageCell[1] == 'X')
                    {
                        physicalDamage = (int)Char.GetNumericValue(damageCell[0]) * 100000;
                    }
                    else if (damageCell[1] == 'M')
                    {
                        physicalDamage = (int)Char.GetNumericValue(damageCell[0]) * 1000000;
                    }
                }

            }

        }
        else if (damageCell.Length == 3)
        {
            //System.out.println("CharAt1: "+Character.getNumericValue(damageCell.charAt(0)));
            //System.out.println("CharAt2: "+damageCell.charAt(1));
            //System.out.println("CharAt3: "+damageCell.charAt(2));
            if (Char.IsDigit(damageCell[1]))
            {

                damageCell = damageCell.Substring(0, damageCell.Length - 1);
                disabled = true;
                physicalDamage = Int32.Parse(damageCell);

            }
            else
            {

                if (damageCell[1] == 'H')
                {
                    physicalDamage = (int)Char.GetNumericValue(damageCell[0]) * 100;
                }
                else if (damageCell[1] == 'K')
                {
                    physicalDamage = (int)Char.GetNumericValue(damageCell[0]) * 1000;
                }
                else if (damageCell[1] == 'T')
                {
                    physicalDamage = (int)Char.GetNumericValue(damageCell[0]) * 10000;
                }
                else if (damageCell[1] == 'X')
                {
                    physicalDamage = (int)Char.GetNumericValue(damageCell[0]) * 100000;
                }
                else if (damageCell[1] == 'M')
                {
                    physicalDamage = (int)Char.GetNumericValue(damageCell[0]) * 1000000;
                }

                disabled = true;

            }

        }

        return (physicalDamage, disabled);
    }

}
}


