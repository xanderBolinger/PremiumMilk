using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x;
    public int y;
    public int z;
    public bool walkable = true;


    // I have absolutely no idea what these variables are for, don't ask
    public int G;
    public int F;
    public int H;
    public Tile Previous;


    public void PrintData() {
        Debug.Log("X: " + x + ", Y: " + y + ", Z: " + z);
    }

    public bool IsBlocked(GameObject mover) {

        foreach (var character in GameObject.FindGameObjectsWithTag("Character")) {
            if (character == mover)
                continue;

            var info = character.GetComponent<CharacterGridInfo>();

            if (info.movingTowardsX == x && info.movingTowardsY == y) 
                return true;
        }

        return false;
    }

    public bool IsBlockedStationary() {
        foreach (var character in GameObject.FindGameObjectsWithTag("Character"))
        {
            var tile = character.GetComponent<CharacterGridInfo>().standingOnTile;
            if (tile.x == x && tile.y == y)
                return true;
        }

        return false;
    }

}
