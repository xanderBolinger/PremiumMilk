using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseController : MonoBehaviour
{


    private void Update()
    {
        var gm = GameManager.Instance;
        if (gm.gridMovement && !gm.turnPaused)
            return;

        CheckGridClick();
    }

    private void CheckGridClick() { 
    
    }

}
