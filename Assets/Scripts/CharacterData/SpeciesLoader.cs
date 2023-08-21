using UnityEngine;
using System;
using System.Collections.Generic;

namespace Character {
    public static class SpeciesLoader
    {
        public static List<Species> LoadSpeciesData()
        {
            List<Species> speciesList = new List<Species>();

            TextAsset csvFile = Resources.Load<TextAsset>("SpeciesData");
            string[] lines = csvFile.text.Split('\n');

            // Skip the header line (assuming it contains the column names)
            for (int i = 1; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(',');

                // Parse the values from the fields
                string name = fields[0];
                string speciesTypeString = fields[1];
                Enum.TryParse(speciesTypeString, out Species.SpeciesType speciesType);
                int arms = int.Parse(fields[2]);
                int legs = int.Parse(fields[3]);
                bool onlyLegs = bool.Parse(fields[4]);

                // Create a new instance of Species
                Species species = new Species(speciesType, arms, legs);

                // Set the remaining fields of the Species instance
                species.speciesName = name;
                species.disabledArms = 0;
                species.disabledLegs = 0;
                species.onlyLegs = onlyLegs;

                // Add the Species instance to the list
                speciesList.Add(species);
            }

            return speciesList;
        }

        public static Species GetSpeciesByName(string speciesName)
        {
            List<Species> speciesList = LoadSpeciesData();

            foreach (Species species in speciesList)
            {
                if (species.speciesName == speciesName)
                {
                    return species;
                }
            }

            throw new Exception("Species not found: " + speciesName);
        }

    }

}


