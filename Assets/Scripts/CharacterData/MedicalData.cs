using System;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class MedicalData
    {

        public int knockoutValue;
        public int criticalTime;
        public int recoveryTargetNumber;
        public bool recivingFirstAid;

        public bool conscious;
        public bool alive;
        public bool mute;
        public bool oneEye;
        public bool blind;


        private int wil;
        List<Injury> injuryList;


        public MedicalData() {
            injuryList = new List<Injury>();
        }

        public MedicalData(Attributes attributes, Species species) { 
            injuryList = new List<Injury>();
            recivingFirstAid = false;
            alive = true;
            conscious = true;

            if(attributes != null) {
                knockoutValue = (int)((double)attributes.str * (double)attributes.hlt * species.GetKoMod());
            }
            wil = attributes.wil;
        }

        public List<Injury> GetInjures() {
            return injuryList;
        }

        public void AddInjury(Injury injury) {
            injuryList.Add(injury);
        }

        public void RemoveInjury(int index) {
            if (index < 0 || index >= injuryList.Count)
                throw new Exception("Index out of bounds for injury list count: "+injuryList.Count+", index: "+index);

            injuryList.RemoveAt(index);
        }

        public void PrintInjuries() {
            Debug.Log("Injuries: ");
            for (int i = 0; i < injuryList.Count; i++) {
                var injury = injuryList[i];
                Debug.Log(i+":: "+injury.ToString());
            }
        }

        public int GetPD() {
            int pd = 0;
            
            foreach (var injury in injuryList) {
                pd += injury.pd;
            }

            return pd;
        }

        public int GetBloodlossPD()
        {
            int blPd = 0;

            foreach (var injury in injuryList)
            {
                blPd += injury.bloodlessPD;
            }

            return blPd;
        }

        public int GetPain()
        {
            int pain = 0;

            foreach (var injury in injuryList)
            {
                pain += injury.pain;
            }

            return (pain - wil / 3) < 0 ? 0 : (pain - wil / 3);
        }

        public bool IsInjured() { return injuryList.Count > 0; }


        public void Knockout(int shockPD)
        {
            int koTN = 0;
            /*int stunnedTN = 0;
            int dazedTN = 0;
            int disorientedTN = 0;*/

            int physicalDamageTotal = GetPD();
            physicalDamageTotal += GetPain() * 5;
            physicalDamageTotal += GetBloodlossPD();
            physicalDamageTotal += shockPD;

            // Rolls incapacitation test 
            if (physicalDamageTotal > knockoutValue * 10)
            {
                koTN = 100;
            }
            else if (physicalDamageTotal > knockoutValue * 5)
            {
                //koTN = 60;
                koTN = 97;
                /*stunnedTN = 94;
                dazedTN = 96;
                disorientedTN = 97;*/
            }
            else if (physicalDamageTotal > knockoutValue * 4)
            {
                //koTN = 26;
                koTN = 66;
                /*stunnedTN = 53;
                dazedTN = 82;
                disorientedTN = 97;*/
            }
            else if (physicalDamageTotal > knockoutValue * 3)
            {
                //koTN = 13;
                koTN = 36;
                /*stunnedTN = 31;
                dazedTN = 52;
                disorientedTN = 74;*/
            }
            else if (physicalDamageTotal > knockoutValue * 2)
            {
                //koTN = 2;
                koTN = 13;
                /*stunnedTN = 8;
                dazedTN = 16;
                disorientedTN = 24;*/
            }
            else
            {
                koTN = 0;
                /*stunnedTN = 2;
                dazedTN = 5;
                disorientedTN = 9;*/
            }

            int roll = DiceRoller.Roll(0, 99);

            if (roll <= koTN)
            {
                conscious = false;
            }

        }
    }

    

}


