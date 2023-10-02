using Ink.Parsed;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Character; 

public class NpcGridMovement : NetworkBehaviour
{
    [SerializeField] int followRange;
    [SerializeField] bool roamer;
    Tile destination;
    PathFinder finder;
    CharacterGridInfo gridInfo;
    GridMover gridMover;
    CharacterAnimator animator;
    CharacterNetwork characterNetwork;
    List<Tile> npcPath;
    CharacterSheet characterSheet;
    public bool moved;
    bool canEnterCombat;


    int startingFollowRange;
    bool startingRoamerStatus;

    Vector3 moveToPosition;
    Tile moveToTile;

    private void Awake()
    {
        characterNetwork = GetComponent<CharacterNetwork>();
        animator = GetComponent<CharacterAnimator>();
        gridMover = GetComponent<GridMover>();
        finder = new PathFinder();
        gridInfo = GetComponent<CharacterGridInfo>();
        npcPath = new List<Tile>();
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(()=> characterNetwork.GetCharacterSheet() != null);

        characterSheet = characterNetwork.GetCharacterSheet();
        startingFollowRange = followRange;
        startingRoamerStatus = roamer;
    }

    private void RotateTowards(Vector3 target)
    {
        Vector3 targetDirection = target - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(targetDirection.x, 0, targetDirection.z));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * (5f));
    }


    private void TranslateCharacter(Vector3 target)
    {
        var step = 3f * Time.deltaTime;

        RotateTowards(target);
        var newPos = Vector3.MoveTowards(transform.position, target, step);
        transform.position = newPos;
    }

    private void Update()
    {
        if(characterSheet == null || characterSheet.meleeCombatStats == null)
            return;


        if (moveToPosition != Vector3.zero && Vector3.Distance(moveToPosition, transform.position) > 0.001f)
        {
            TranslateCharacter(moveToPosition);
            RotateTowards(moveToPosition);
            if (gridMover.jumper)
                animator.SetJumping();
            else
                animator.SetWalk();
            return;
        }
        else if (moveToTile != null && gridInfo.standingOnTile != moveToTile) {
            gridMover.EndCharacterMovement(moveToTile, gridInfo, moveToPosition);
        }
        else if ((animator.attacking && animator.attackFinished) || !animator.attacking)
        {
            animator.SetIdle();
        }

        canEnterCombat = characterSheet.meleeCombatStats.GetMaxCp(characterSheet.medicalData.GetPain(), characterSheet.fatigueSystem.fatiguePoints) > 4;


        if (!canEnterCombat)
        {
            followRange = 0;
            roamer = true;
        }
        else {
            roamer = startingRoamerStatus;
            followRange = startingFollowRange;
        }

        if (!isServer)
        {
            return;
        }
        else if (GameManager.Instance.playerDied) {
            animator.SetIdle();
            return;
        }
    }

    public void SetDestination() {
        var (enemy, dist) = GetNearestEnemy();

        if (dist > 20) {
            Debug.Log("Set destination early dist return");
            return;
        }

        if (dist == 1 && canEnterCombat) {
            GetComponent<CharacterCombatController>().EnterCombat(enemy);
            return;
        }
        else if (dist < followRange)
        {
            MoveToTarget(enemy);
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
        moveToTile = npcPath[0];
        moveToPosition = npcPath[0].gameObject.transform.position;
        moveToPosition.y += 1.5f;
        npcPath.RemoveAt(0);
        moved = true;
    }

    public bool FinishedMovement() { 
        if(destination == null)
            return false;
        return destination == gridInfo.standingOnTile;
    }

    void MoveToTarget(GameObject enemy) {
        var neighbours = finder.GetNeightbourTiles(enemy.GetComponent<CharacterGridInfo>().standingOnTile);

        List<Tile> validNeighbours = new List<Tile>();

        float distance = float.MaxValue;
        Tile closestTile = null;

        foreach (var tile in neighbours) {
            if (tile.walkable)
            {
                validNeighbours.Add(tile);
                var tempDist = Vector3.Distance(tile.gameObject.transform.position, gameObject.transform.position);
                if (tempDist < distance) {
                    closestTile = tile;
                    distance = tempDist;
                }

            }
        }

        if (closestTile == null)
            throw new System.Exception("Closest tile to target is null.");

        destination = closestTile;
        npcPath = finder.FindPath(gridInfo.standingOnTile, destination);
    }

    public void SetRandomPath() {
        iterations = 0;
        var map = MapManager.Instance.map.ToList();
        List<Tile> validDestinations = new List<Tile>();
        GetChildren(gridInfo.standingOnTile.gameObject, map, validDestinations);

        if (validDestinations.Count < 1)
            return;

        destination = validDestinations[roamer ? (validDestinations.Count - 1) :
            DiceRoller.Roll(0, validDestinations.Count-1)];
        npcPath = finder.FindPath(gridInfo.standingOnTile, destination);

    }

    int iterations;
    public void GetChildren(GameObject parent, List<KeyValuePair<Vector2Int, Tile>> t, List<Tile> returnList)
    {
        iterations++;
        if (iterations >= 15)
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
