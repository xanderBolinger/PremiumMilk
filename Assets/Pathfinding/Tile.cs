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

    public bool IsBlocked() {
        return false;
    }

}