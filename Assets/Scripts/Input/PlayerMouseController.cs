using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseController : NetworkBehaviour
{
    private GridMover gridMover;

    void Start() { 
        gridMover = GetComponent<GridMover>();
    }

    private void Update()
    {
        var gm = GameManager.Instance;
        var tile = gridMover.GetHitTile();

        if (!isLocalPlayer 
            || (gm.turnBasedMovement && !gm.turnPaused) 
            || tile == null)
            return;

        var newPath = gridMover.GetNewPath(tile);
        gridMover.PlotPath(newPath);
        gridMover.ConfirmNewPath(newPath);
    }

}
