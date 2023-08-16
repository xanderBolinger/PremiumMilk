using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{


    public void DeactivateAll() { 
        GetComponent<IdleAnimator>()?.Deactivate();
        GetComponent<SwingAnimator>()?.Deactivate();
    }



}
