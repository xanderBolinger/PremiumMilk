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

    private List<Tile> tiles = FindObjectsOfType<Tile>().ToList();

    //return script on tile GameObject.GetCompenet<Tile>

    public void StartAlgorithm() {
        GetChildren(StartingObject, tiles);
    }
    public void GetChildren(GameObject parent, List<Tile> t) {

        Vector3 parentLocation = parent.transform.position;
        var parentTile = parent.GetComponent<Tile>();

        int parentX = parentTile.x;
        int parentY = parentTile.y;
        int count = 0;

        for (int i = 0; i < t.Count; i++) {
            var child = t[i].GetComponent<Tile>();

            float difference = Mathf.Abs(Vector3.Distance(t[i].transform.position, parentLocation));

            if (difference < 1.500001) {
                count++;
                Vector3 d = parentLocation - (t[i].transform.position);
                if (d.x < 0 && d.z == 0) {
                    child.x = parentX - 1; //Left
                    child.y = parentY;
                    t.RemoveAt(i);
                    GetChildren(t[i].gameObject, t);
                } else if (d.x < 0 && d.z == 0) { 
                    child.x = parentX + 1; //Right
                    child.y = parentY;
                    t.RemoveAt(i);
                    GetChildren(t[i].gameObject, t);
                } else if (d.x == 0 && d.z < 0) {
                    child.x = parentX;     // down
                    child.y = parentY - 1;
                    t.RemoveAt(i);
                    GetChildren(t[i].gameObject, t);
                } else if (d.x == 0 && d.z > 0) {
                    child.x = parentX;     // up
                    child.y = parentY + 1;
                    t.RemoveAt(i);
                    GetChildren(t[i].gameObject, t);
                }
            }

            if (count == 4) break;
        }
    }

}
