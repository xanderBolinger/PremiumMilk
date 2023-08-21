using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Character {
    public class Species
    {
        public enum SpeciesType
        {
            MINISCULE, SMALL, MEDIUM_SIZE, LARGE, VERY_LARGE, GIANT
        }

        public SpeciesType speciesType;
        public int arms;
        public int disabledArms;
        public int legs;
        public int disabledLegs;
        public bool onlyLegs;
        public string speciesName;

        public Species(SpeciesType speciesType, int arms, int legs) {
            this.speciesType = speciesType;
            this.arms = arms;
            this.legs = legs;
        }

        // Empty constructor for testing 
        public Species() { }

        public bool canFight() {
            return canWalk();
        }

        public bool canUseTwoHanded() {
            int limbs;
            int disabledLimbs;
            if (onlyLegs)
            {
                limbs = legs;
                disabledLimbs = disabledLegs;
            }
            else {
                limbs = arms;
                disabledLimbs = disabledArms;
            }

            return limbs - disabledLimbs >= 2;
        }

        public double GetKoMod()
        {
            switch (speciesType)
            {
                case SpeciesType.MINISCULE:
                    return 0.25;
                case SpeciesType.SMALL:
                    return 0.5;
                case SpeciesType.MEDIUM_SIZE:
                    return 1;
                case SpeciesType.LARGE:
                    return 4;
                case SpeciesType.VERY_LARGE:
                    return 5;
                case SpeciesType.GIANT:
                    return 10;
            }

            throw new Exception("Species type not found for: "+speciesType);
        }
        
        public bool canRun() {
            return legs - disabledLegs >= Math.Ceiling( 0.6f * (float)legs);
        }
        public bool canWalk() {
            return legs - disabledLegs >= Math.Ceiling(0.5f * (float)legs);
        }


        public static int GetDamageLevel(SpeciesType species, int damagePoints) {

            string fileName = GetFileName(species);

            TextAsset damageLevelData = Resources.Load<TextAsset>(fileName);

            string[] data = damageLevelData.text.Split(new char[] { '\n' });

            for (int i = 0; i < data.Length; i++) {
                string[] row = data[i].Split(new char[] { ','});
                int.TryParse(row[0], out int row0);
                int.TryParse(row[1], out int row1);
                if (damagePoints >= row0) {
                    return row1;
                }

            }

            throw new Exception("Damage Level Not found for species: "+species+", or damage points: "+damagePoints);

        }

        public static int GetDamageValue(SpeciesType species, int level)
        {

            string fileName = GetFileName(species);

            TextAsset damageLevelData = Resources.Load<TextAsset>(fileName);

            string[] data = damageLevelData.text.Split(new char[] { '\n' });

            for (int i = 0; i < data.Length; i++)
            {
                string[] row = data[i].Split(new char[] { ',' });
                int.TryParse(row[0], out int row0);
                int.TryParse(row[1], out int row1);
                if (level >= row1)
                {
                    return row0;
                }

            }

            throw new Exception("Damage Value Not found for species: " + species + ", or level: " + level);

        }

        private static string GetFileName(SpeciesType speciesType)
        {
            switch (speciesType)
            {
                case SpeciesType.MINISCULE:
                    return "DamageLevelMiniscule";
                case SpeciesType.SMALL:
                    return "DamageLevelSmall";
                case SpeciesType.MEDIUM_SIZE:
                    return "DamageLevelMedium";
                case SpeciesType.LARGE:
                    return "DamageLevelLarge";
                case SpeciesType.VERY_LARGE:
                    return "DamageLevelVeryLarge";
                case SpeciesType.GIANT:
                    return "DamageLevelGiant";
                default:
                    throw new System.Exception("Species filename not found for species: " + speciesType);
            }

        }

    }
}
