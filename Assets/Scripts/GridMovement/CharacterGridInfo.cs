using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CharacterGridInfo : NetworkBehaviour
{
    [SerializeField] bool randomStart;

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

    private IEnumerator Start()
    {

        yield return new WaitUntil(() => MapManager.Instance != null &&
        MapManager.Instance.setTiles != null && MapManager.Instance.setTiles.finished
        && MapManager.Instance.map != null
        && MapManager.Instance.map.ContainsKey(new Vector2Int(startingX, startingY)));

        if (randomStart) {
            var list = MapManager.Instance.map.Select(tile => tile.Key).ToList();

            var validList = new List<Vector2Int>();

            foreach (var t in MapManager.Instance.map) {
                if (t.Value.walkable)
                    validList.Add(t.Key);
            }

            var locationKey = validList[DiceRoller.Roll(0, validList.Count - 1)];
            startingX = locationKey.x;
            startingY = locationKey.y;
        }


        if (standingOnTile == null && MapManager.Instance != null) {
            standingOnTile = MapManager.Instance.GetTile(startingX, startingY);
            var newPos = standingOnTile.transform.position;
            transform.position = new Vector3(newPos.x, newPos.y + 1.5f, newPos.z);
            if (isOwned)
            {
                CmdSetStandingOnTile(standingOnTile.x, standingOnTile.y);
                CmdSetMovingTowards(-1, -1);
            }
            else {
                movingTowardsX = -1;
                movingTowardsY = -1;
                standingOnX = standingOnTile.x;
                standingOnY = standingOnTile.y;
            }
            
        }
    }

    [Command]
    public void CmdClearMovingTowards() {
        movingTowardsX = -1;
        movingTowardsY = -1;
    }

    public void ClearMovingTowards() {
        movingTowardsX = -1;
        movingTowardsY = -1;
    }

    [Command]
    public void CmdSetMovingTowards(int x, int y) {
        SetMovingTowards(x, y);
    }

    public void SetMovingTowards(int x, int y)
    {
        movingTowardsX = x;
        movingTowardsY = y;
    }

    [Command]
    public void CmdSetStandingOnTile(int x, int y) {
        standingOnX = x;
        standingOnY = y;
    }

    public void SetStandingOnTile(int x, int y) {
        standingOnX = x;
        standingOnY = y;
    }

}

