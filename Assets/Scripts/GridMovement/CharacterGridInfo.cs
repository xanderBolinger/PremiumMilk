using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterGridInfo : MonoBehaviour
{
    public Tile standingOnTile;

    public int startingX;
    public int startingY;
    public int movingTowardsX { get; private set; }
    public int movingTowardsY { get; private set; }

    private void Start()
    {
        if (standingOnTile == null && MapManager.Instance != null) {
            standingOnTile = MapManager.Instance.GetTile(startingX, startingY);
            var newPos = standingOnTile.transform.position;
            transform.position = new Vector3(newPos.x, newPos.y + 1, newPos.z);
        }
    }

    public void ClearMovingTowards() {
        movingTowardsX = -1;
        movingTowardsY = -1;
    }

    public void SetMovingTowards(int x, int y) {
        movingTowardsX = x;
        movingTowardsY = y;
    }

}

