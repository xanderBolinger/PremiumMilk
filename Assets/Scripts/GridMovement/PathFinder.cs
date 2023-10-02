using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PathFinder
{
    public List<Tile> FindPath(Tile start, Tile end)
    {
        if (Vector3.Distance(start.gameObject.transform.position, end.gameObject.transform.position) > 8)
            return new List<Tile>();

        //Debug.Log("Start: "+start.x+", "+start.y+", End: "+end.x+", "+end.y);
        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        openList.Add(start);

        while (openList.Count > 0)
        {
            Tile currentTile = openList.OrderBy(x => x.F).First();

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            if (currentTile == end)
            {
                return GetFinishedList(start, end);
            }
            

            foreach (var tile in GetNeightbourTiles(currentTile))
            {
                if (tile.IsBlockedStationary() || !tile.walkable || closedList.Contains(tile)
                    /* || Mathf.Abs(currentTile.transform.position.z - tile.transform.position.z) > 1*/)
                {
                    continue;
                }

                tile.G = GetManhattenDistance(start, tile);
                tile.H = GetManhattenDistance(end, tile);

                tile.Previous = currentTile;


                if (!openList.Contains(tile))
                {
                    openList.Add(tile);
                }
            }
        }

        return new List<Tile>();
    }

    private List<Tile> GetFinishedList(Tile start, Tile end)
    {
        List<Tile> finishedList = new List<Tile>();
        Tile currentTile = end;

        while (currentTile != start)
        {
            finishedList.Add(currentTile);
            currentTile = currentTile.Previous;
        }

        finishedList.Reverse();

        return finishedList;
    }

    private int GetManhattenDistance(Tile start, Tile tile)
    {
        return Mathf.Abs(start.x - tile.x) + Mathf.Abs(start.y - tile.y);
    }

    public static int Distance(int x1, int x2, int y1, int y2)
    {
        return Mathf.Max(Mathf.Abs(x1 - x2), Mathf.Abs(y1 - y2));
    }


    public List<Tile> GetNeightbourTiles(Tile currentTile)
    {
        //Debug.Log("Neighbour Tile: "+currentTile.x+", "+currentTile.y);
        var map = MapManager.Instance.map;

        List<Tile> neighbours = new List<Tile>();

        //right
        Vector2Int locationToCheck = new Vector2Int(
            currentTile.x + 1,
            currentTile.y
        );

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        //left
        locationToCheck = new Vector2Int(
            currentTile.x - 1,
            currentTile.y
        );

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        //top
        locationToCheck = new Vector2Int(
            currentTile.x,
            currentTile.y + 1
        );

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        //bottom
        locationToCheck = new Vector2Int(
            currentTile.x,
            currentTile.y - 1
        );

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        return neighbours;
    }

      
}
