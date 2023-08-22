using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseController : MonoBehaviour
{
    private GridMover gridMover;

    void Start() { 
        gridMover = GetComponent<GridMover>();
    }

    private void Update()
    {
        var gm = GameManager.Instance;

        if (gm.gridMovement && !gm.turnPaused)
            return;

        var tile = gridMover.GetHitTile();

        if (tile == null)
            return;

        var newPath = gridMover.GetNewPath(tile);
        gridMover.PlotPath(newPath);
        gridMover.ConfirmNewPath(newPath);
    }

}
