using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class Algorithm : MonoBehaviour {

    [SerializeField] GameObject StartingObject;

    public List<GameObject> zeroTiles;

    private List<Tile> tiles;

    //return script on tile GameObject.GetCompenet<Tile>

    int iterations;

    public bool finished;

    private void Start()
    {
        var t = GameObject.FindObjectsOfType<Tile>();
        tiles = t.ToList();
        StartAlgorithm();

        zeroTiles = new List<GameObject>();

        foreach (var tile in t) {
            var cords = tile.GetComponent<Tile>();
            if (cords.x == 0 && cords.y == 0) {
                zeroTiles.Add(tile.gameObject);
                //throw new System.Exception("Tile at: "+tile.transform.position+", is 0:0");
            }
        }

        if(zeroTiles.Count > 0)
            throw new System.Exception(zeroTiles.Count+" tiles are 0,0 and were probably not reached by algorithim.");

        finished = true;
    }

    public void StartAlgorithm() {
        GetChildren(StartingObject, tiles);
    }
    public void GetChildren(GameObject parent, List<Tile> t) {
        iterations++;
        if (iterations >= 10000)
            return;

        Vector3 parentLocation = parent.transform.position;
        var parentTile = parent.GetComponent<Tile>();

        int parentX = parentTile.x;
        int parentY = parentTile.y;
        int count = 0;

        for (int i = 0; i < t.Count; i++) {
            Tile child = t[i];

            if (child.x != 0 && child.y != 0)
                continue;

            float difference = Mathf.Abs(Vector3.Distance(child.transform.position, parentLocation));

            if (difference < 1.500001) {
                count++;
                Vector3 d = -(parentLocation - (child.transform.position));
                if (d.x < 0 && d.z == 0) {
                    child.x = parentX - 1; //Left
                    child.y = parentY;
                    //t.RemoveAt(i);
                    GetChildren(child.gameObject, t);
                } else if (d.x > 0 && d.z == 0) { 
                    child.x = parentX + 1; //Right
                    child.y = parentY;
                    //t.RemoveAt(i);
                    GetChildren(child.gameObject, t);
                } else if (d.x == 0 && d.z < 0) {
                    child.x = parentX;     // down
                    child.y = parentY - 1;
                    //t.RemoveAt(i);
                    GetChildren(child.gameObject, t);
                } else if (d.x == 0 && d.z > 0) {
                    child.x = parentX;     // up
                    child.y = parentY + 1;
                    //t.RemoveAt(i);
                    GetChildren(child.gameObject, t);
                }

            }

            if (count == 4) {
                break;
            } 
        }
    }

}
