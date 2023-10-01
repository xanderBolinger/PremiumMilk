using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    public static MapManager Instance { get { return _instance; } }

    public float littleBump;

    public Dictionary<Vector2Int, Tile> map;

    public Algorithm setTiles;

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


        foreach (var tile in GameObject.FindGameObjectsWithTag("Tile")) {
            var data = tile.GetComponent<Tile>();
            map.Add(new Vector2Int(data.x, data.y), data);
        }

    }

    public Tile GetTile(int x, int y) {
        
        foreach (var tile in GameObject.FindGameObjectsWithTag("Tile")) {
            var data = tile.GetComponent<Tile>();
            if (data.x == x && data.y == y) {
                return data;
            }
        }


        throw new System.Exception("Tile not found for x: " + x + ", y: " + y);
    }


}

