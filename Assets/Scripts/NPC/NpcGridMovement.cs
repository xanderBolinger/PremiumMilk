using Ink.Parsed;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NpcGridMovement : NetworkBehaviour
{
    [SerializeField] int followRange;
    [SerializeField] bool roamer;
    Tile destination;
    PathFinder finder;
    CharacterGridInfo gridInfo;
    GridMover gridMover;
    CharacterAnimator animator;

    List<Tile> npcPath;

    public bool moved;

    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
        gridMover = GetComponent<GridMover>();
        finder = new PathFinder();
        gridInfo = GetComponent<CharacterGridInfo>();
        npcPath = new List<Tile>();
    }

    private void Update()
    {
        if (!isServer || GameManager.Instance.playerDied)
            return;


        var moving = gridMover.Moving();

        if (moving && gridMover.canMoveAnimation && GameManager.Instance.playGridMovment
            && (GameManager.Instance.turnBasedMovement && gridMover.movementTurn))
        {
            gridMover.MoveAlongPath();
            if (gridMover.jumper)
                animator.SetJumping();
            else
                animator.SetWalk();
        }
        else if (!moving)
        {
            gridMover.canMoveAnimation = true;
            gridMover.MovementNotReady();
            gridMover.movementTurn = false;
            animator.SetIdle();
        }
    }

    public void SetDestination() {
        var (enemy, dist) = GetNearestEnemy();

        if (dist == 1) {
            GetComponent<CharacterCombatController>().EnterCombat(enemy);
            return;
        }
        else if (dist < followRange)
        {
            MoveToTarget(enemy);
            npcPath = gridMover.path;
        } 
        
        if (npcPath.Count < 1 || FinishedMovement())
        {
            gridInfo.ClearMovingTowards();
            SetRandomPath();
            return;
        }

        var tile = npcPath[0];

        if (tile.IsBlocked(gameObject))
        {
            npcPath.Clear();
            gridInfo.ClearMovingTowards();
            return;
        }

        gridInfo.SetMovingTowards(tile.x, tile.y);
        gridMover.path.Add(npcPath[0]);
        npcPath.RemoveAt(0);
        moved = true;
    }

    public bool FinishedMovement() { 
        if(destination == null)
            return false;
        return destination == gridInfo.standingOnTile;
    }

    void MoveToTarget(GameObject enemy) {
        destination = finder.GetNeightbourTiles(enemy.GetComponent<CharacterGridInfo>().standingOnTile)[0];
        gridMover.path = finder.FindPath(gridInfo.standingOnTile, destination);
    }

    public void SetRandomPath() {
        iterations = 0;
        var map = MapManager.Instance.map.ToList();
        List<Tile> validDestinations = new List<Tile>();
        GetChildren(gridInfo.standingOnTile.gameObject, map, validDestinations);

        destination = validDestinations[roamer ? (validDestinations.Count - 1) :
            DiceRoller.Roll(0, validDestinations.Count-1)];
        npcPath = finder.FindPath(gridInfo.standingOnTile, destination);

    }

    int iterations;
    public void GetChildren(GameObject parent, List<KeyValuePair<Vector2Int, Tile>> t, List<Tile> returnList)
    {
        iterations++;
        if (iterations >= 100)
            return;

        Vector3 parentLocation = parent.transform.position;

        int count = 0;

        for (int i = 0; i < t.Count; i++)
        {
            Tile child = t[i].Value;



            float difference = Mathf.Abs(Vector3.Distance(child.transform.position, parentLocation));

            if (difference < 1.500001)
            {
                count++;
                Vector3 d = -(parentLocation - (child.transform.position));
                if ((d.x < 0 && d.z == 0 || d.x > 0 && d.z == 0 || d.x == 0 && d.z < 0
                    || d.x == 0 && d.z > 0) && !returnList.Contains(child) && child.walkable)
                {
                    returnList.Add(child);
                    GetChildren(child.gameObject, t, returnList);
                }
                

                if (count == 4)
                {
                    break;
                }
            }
        }
    }


    public (GameObject, int) GetNearestEnemy() {

        int distance = int.MaxValue;
        GameObject closestEnemey = null;

        foreach (var character in GameObject.FindGameObjectsWithTag("Character")) { 
            if(character == gameObject)
                continue;
            if (!character.GetComponent<CharacterController>().player)
                continue;

            var tile = character.GetComponent<CharacterGridInfo>();

            var tempDistance = PathFinder.Distance(tile.standingOnX, gridInfo.standingOnX, 
                tile.standingOnY, gridInfo.standingOnY);

            if (tempDistance < distance) {
                closestEnemey = character;
                distance = tempDistance;
            }

        }


        return (closestEnemey, distance);
    }

}
