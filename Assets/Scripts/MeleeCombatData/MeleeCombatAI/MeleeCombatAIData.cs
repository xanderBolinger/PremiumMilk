using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCombatAIData : MonoBehaviour
{
    public enum BehaviorType { 
        Agressive,Cowardly,Measured,Feral
    }

    public BehaviorType behavior;

}
