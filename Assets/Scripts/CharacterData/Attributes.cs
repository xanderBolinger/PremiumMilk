using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character {

    public class Attributes
    {

        public int str;
        public int hlt;
        public int agl;
        public int per;
        public int wil;

        public Attributes(int str, int hlt, int agl, int per, int wil)
        {
            this.str = str;
            this.hlt = hlt;
            this.agl = agl;
            this.per = per;
            this.wil = wil;
        }

        public Attributes() { }

    }

}




