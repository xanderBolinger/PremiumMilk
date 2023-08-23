using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterGridInfo : NetworkBehaviour
{
    // This is a variable that is only set on the client that owns it
    // and will not be accurate on the server or other clients
    public Tile standingOnTile;

    public int startingX;
    public int startingY;

    [SyncVar]
    public int movingTowardsX;
    [SyncVar]
    public int movingTowardsY;

    [SyncVar]
    public int standingOnX;
    [SyncVar]
    public int standingOnY;

    private void Start()
    {
        if (standingOnTile == null && MapManager.Instance != null) {
            standingOnTile = MapManager.Instance.GetTile(startingX, startingY);
            var newPos = standingOnTile.transform.position;
            transform.position = new Vector3(newPos.x, newPos.y + 1, newPos.z);
            CmdSetStandingOnTile(standingOnTile.x, standingOnTile.y);
            CmdSetMovingTowards(-1, -1);
        }
    }

    [Command]
    public void CmdClearMovingTowards() {
        movingTowardsX = -1;
        movingTowardsY = -1;
    }

    [Command]
    public void CmdSetMovingTowards(int x, int y) {
        movingTowardsX = x;
        movingTowardsY = y;
    }

    [Command]
    public void CmdSetStandingOnTile(int x, int y) {
        standingOnX = x;
        standingOnY = y;
    }

}

