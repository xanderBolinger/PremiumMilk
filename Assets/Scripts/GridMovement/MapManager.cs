using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    public static MapManager Instance { get { return _instance; } }

    public float littleBump;

    public Dictionary<Vector2Int, Tile> map;

    public SetTilesAlgorithm setTiles;

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {

        yield return new WaitUntil(() => setTiles.finished);

        littleBump = 0.0003f;
        map = new Dictionary<Vector2Int, Tile>();

        int lowestX = int.MaxValue;
        int lowestY = int.MaxValue;
        int highestX = int.MinValue;
        int highestY = int.MinValue;

        foreach (var tile in GameObject.FindGameObjectsWithTag("Tile")) {
            var data = tile.GetComponent<Tile>();
            map.Add(new Vector2Int(data.x, data.y), data);

            if (data.x >= highestX && data.y >= highestY) {
                highestX = data.x;
                highestY = data.y;
            }
            if (data.x <= lowestX && data.y <= lowestY) {
                lowestX = data.x;
                lowestY = data.y;
            }
        }


        for (int x = lowestX; x < highestX; x++) {

            for (int y = lowestY; y < highestY; y++) {

                var vector = new Vector2Int(x, y);

                if (map.ContainsKey(vector))
                    continue;
                else {
                    GameObject obj = new GameObject("MissingTile "+x+":"+y);
                    var newTile = obj.AddComponent<Tile>();
                    newTile.walkable = false;
                    Instantiate(obj, transform);
                    newTile.x = x;
                    newTile.y = y;
                    map.Add(vector, newTile);
                }

            }

        }



    }

    public Tile GetTile(int x, int y) {

        return map[new Vector2Int(x, y)];


        //throw new System.Exception("Tile not found for x: " + x + ", y: " + y);
    }


}

