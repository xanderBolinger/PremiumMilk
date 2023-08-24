using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character {
    public class Injury
    {

        public int pd;
        public int bloodlessPD;
        public int pain;
        public int shock;
        public string weaponDealt;
        public string charDealt;
        public string anatomicalLocation;
        public int level;

        public Injury() { }

        public Injury(int pD, int bloodlessPD, int pain, int shock, string weaponDealt, 
            string charDealt, string anatomicalLocation, int level)
        {
            this.pd = pD;
            this.bloodlessPD = bloodlessPD;
            this.pain = pain;
            this.shock = shock;
            this.weaponDealt = weaponDealt;
            this.charDealt = charDealt;
            this.anatomicalLocation = anatomicalLocation;
            this.level = level;
        }

        public override string ToString()
        {
            return "Injury("+charDealt+", "+weaponDealt+"), "+"Location: "+anatomicalLocation+"Level "+level
                +",pd: "+pd+", Blood Loss PD: "+bloodlessPD+", Pain: "+pain+", Shock: "+shock;
        }

    }
}

