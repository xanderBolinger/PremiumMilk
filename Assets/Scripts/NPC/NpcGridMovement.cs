using Ink.Parsed;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NpcGridMovement : NetworkBehaviour
{
    [SerializeField] int followRange;

    Tile destination;
    PathFinder finder;
    CharacterGridInfo gridInfo;
    GridMover gridMover;
    CharacterAnimator animator;



    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
        gridMover = GetComponent<GridMover>();
        finder = new PathFinder();
        gridInfo = GetComponent<CharacterGridInfo>();
    }

    private void Update()
    {
        if (!isServer)
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
        var path = gridMover.path;
        var (enemy, dist) = GetNearestEnemy();

        if (dist == 1) {
            GetComponent<CharacterCombatController>().EnterCombat(enemy);
            return;
        }
        else if (dist < followRange)
        {
            MoveToTarget(enemy);
        }

        if (path.Count < 1 || FinishedMovement())
        {
            gridInfo.ClearMovingTowards();
            SetRandomPath();
            return;
        }

        var tile = path[0];

        if (tile.IsBlocked(gameObject))
        {
            path.Clear();
            gridInfo.ClearMovingTowards();
            return;
        }

        gridInfo.SetMovingTowards(tile.x, tile.y);
    }

    public bool FinishedMovement() { 
        if(destination == null)
            return false;
        return destination == gridInfo.standingOnTile;
    }

    void MoveToTarget(GameObject enemy) {
        destination = enemy.GetComponent<CharacterGridInfo>().standingOnTile;
        gridMover.path = finder.FindPath(gridInfo.standingOnTile, destination);
    }

    public void SetRandomPath() {
        var map = MapManager.Instance.map.ToList();

        destination = map[DiceRoller.Roll(0, map.Count - 1)].Value;

        gridMover.path = finder.FindPath(gridInfo.standingOnTile, destination);

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
