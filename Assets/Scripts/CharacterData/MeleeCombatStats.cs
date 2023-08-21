using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character {

    public class MeleeCombatStats {

        public int reflexes = 0;
        public int combatActions = 4;
        public int profValue { private set; get; }
        public MeleeProficiencies.MeleeProfType currProf;
        public List<ArmorPiece> armorPieces;
        public MeleeWeaponStatBlock weapon;
        public MeleeShield shield;

        private Dictionary<MeleeProficiencies.MeleeProfType, int> learnedProficiencies;


        public MeleeCombatStats() {
            learnedProficiencies = new Dictionary<MeleeProficiencies.MeleeProfType, int>();
            armorPieces = new List<ArmorPiece>();
            LearnProficiency(MeleeProficiencies.CutNThrust, 0);
        }

        public MeleeCombatStats(MeleeCombatStats stats) { 
            learnedProficiencies = stats.learnedProficiencies.ToDictionary(entry => entry.Key,
                                               entry => entry.Value);
            armorPieces = new List<ArmorPiece>(stats.armorPieces);
            currProf = stats.currProf;
            profValue = stats.profValue;
            weapon = stats.weapon;
            shield = stats.shield;
            reflexes = stats.reflexes;
        }

        public Dictionary<MeleeProficiencies.MeleeProfType, int> GetProficiencies() {
            return learnedProficiencies;
        }

        public void LearnProficiency(Proficiency prof, int value) {
            if (learnedProficiencies.ContainsKey(prof.meleeProfType))
                learnedProficiencies[prof.meleeProfType] = value;
            else { 
                learnedProficiencies.Add(prof.meleeProfType, value);
                currProf = prof.meleeProfType;
            }

            if(currProf == prof.meleeProfType)
                profValue = value;
        }

        public void SetCurrProf(MeleeProficiencies.MeleeProfType meleeProfType) {

            currProf = meleeProfType;
            
            if (learnedProficiencies.ContainsKey(meleeProfType))
            {
                profValue = learnedProficiencies[meleeProfType];
            }
            else
            {
                int maxProfValue = int.MinValue;

                foreach (var learnedProf in learnedProficiencies)
                {
                    int defaultValue = MeleeProficiencies.GetProfByType(learnedProf.Key)
                        .GetDefaultValue(meleeProfType) + learnedProf.Value;
                    if (defaultValue > maxProfValue)
                        maxProfValue = defaultValue;
                }

                if (maxProfValue == int.MinValue)
                    throw new System.Exception("Max prof value is still integer min value.");

                profValue = maxProfValue;
            }

        }

        public void SetCurrProf(Proficiency currProf) {

            this.currProf = currProf.meleeProfType;
        }

        public int GetMaxCp(int pain) {
            return reflexes + profValue - pain;
        }

        public void CalcReflexes(Attributes a) {
            reflexes = (a.str + a.agl + a.per) / 9;
        }

        public int GetKnockDown(Attributes a) {
            return (a.str/3 + a.agl/3) / 2;
        }

        public Proficiency GetProf() {
            return MeleeProficiencies.GetProfByType(currProf);
        }

        public void EquipArmor(string name) {
            var armorPiece = ArmorLoader.GetArmorPieceByName(name);
            armorPieces.Add(armorPiece);
        }

        public void UnequipArmor(string name) {
            ArmorPiece armorPiece = null;

            foreach(var ap in armorPieces)
            {
                if (ap.Name == name) { 
                    armorPiece = ap;
                    break;
                }
            }

            armorPieces.Remove(armorPiece);
        }

        public void EquipShield(string name) {
            shield = MeleeShieldLoader.GetShieldByName(name);
        }

        public void UnequipShield() {
            shield = null;
        }

        public void EquipWeapon(string name) {
            weapon = MeleeWeaponLoader.GetWeaponByName(name);
        }

        public void UnequipWeapon() {
            weapon = null;
        }
    }
}
