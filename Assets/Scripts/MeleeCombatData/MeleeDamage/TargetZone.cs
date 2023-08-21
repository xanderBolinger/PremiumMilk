using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetZone
{

    public enum TargetZoneCutting { 
        LowerLegs=1,
        UpperLegs=2,
        MidBody=3, 
        OverHand=4,
        VerticalDown=5, 
        VerticalUp=6,
        Arms=7
    }

    public enum TargetZonePuncture
    {
        LowerLegs = 1,
        UpperLegs = 2,
        PelvicRegion = 3,
        Abdomen = 4,
        Chest = 5,
        Head = 6,
        Arms = 7
    }

}
