using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Character {

    public class CharacterSheet
    {
        public string name;
        public Attributes attributes;
        public Species species;
        public MedicalData medicalData;
        public MeleeCombatStats meleeCombatStats;
        public FatigueSystem fatigueSystem;

        public CharacterSheet() { }

        public CharacterSheet(CharacterSheet characterSheet)
        {
            name = characterSheet.name;
            attributes = characterSheet.attributes;
            species = characterSheet.species;
            medicalData = characterSheet.medicalData;
            meleeCombatStats = new MeleeCombatStats(characterSheet.meleeCombatStats);
            fatigueSystem = characterSheet.fatigueSystem;
        }

        public CharacterSheet(string name, Species species, Attributes attributes, MedicalData medicalData, MeleeCombatStats meleeCombatStats) {
            this.name = name;
            this.species = species;
            this.attributes = attributes;
            this.medicalData = medicalData;
            this.meleeCombatStats = meleeCombatStats;
            this.fatigueSystem = new FatigueSystem(attributes);
        }

        public bool Alive() {
            return medicalData.conscious && medicalData.alive;
        }

    }

}
