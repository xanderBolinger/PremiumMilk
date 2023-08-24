using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEditor;
using Mirror;
using System.Runtime.CompilerServices;

[RequireComponent(typeof(CharacterGridInfo))]
public class GridMover : NetworkBehaviour
{
    //public GameObject cursor;
    public float speed = 1f;
    private CharacterGridInfo character;
    CharacterAnimator animator;

    private PathFinder pathFinder;
    public List<Tile> path;
    private List<Tile> plottedPath;

    public bool canMoveAnimation = true;
    public bool jumper = false;

    [SyncVar]
    public bool movementReady = false;
    [SyncVar]
    public bool movementTurn = false;

    private void Start()
    {
        character = GetComponent<CharacterGridInfo>();
        animator = GetComponent<CharacterAnimator>();
        pathFinder = new PathFinder();
        path = new List<Tile>();
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        var moving = Moving();

        if (moving && canMoveAnimation && GameManager.Instance.playGridMovment 
            && (GameManager.Instance.turnBasedMovement && movementTurn))
        {
            MoveAlongPath();
            if (jumper)
                animator.SetJumping();
            else
                animator.SetWalk();
        }
        else if (!moving) {
            canMoveAnimation = true;
            MovementNotReady();
            movementTurn = false;
            animator.SetIdle();
        }

    }

    private void MovementNotReady() {
        CmdMovementNotReady();
    }

    [Command]
    private void CmdMovementNotReady()
    {
        movementReady = false;
    }

    private void MovementReady() {
        CmdMovementReady();
    }

    [Command]
    private void CmdMovementReady() { 
        movementReady = true;
    }

    private bool Moving() {
        return path.Count > 0;
    }

    public Tile GetHitTile() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, 100) || hit.transform.gameObject.tag != "Tile")
        {
            return null;
        }

        return hit.transform.gameObject.GetComponent<Tile>();
    }

    public List<Tile> GetNewPath(Tile tile) {
        return pathFinder.FindPath(character.standingOnTile, tile);
    }

    public void ConfirmNewPath(List<Tile> newPath) {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }
        PlayConfirmPath();
        path = newPath;
        MovementReady();
    }


    public void CanMoveTrue() {
        canMoveAnimation = true;
    }

    public void CanMoveFalse() {
        canMoveAnimation = false;
    }

    private void PlayMovedTarget() {
        if (path.Count != 0 && plottedPath.Count != 0) {
            var t1 = path[path.Count - 1];
            var t2 = plottedPath[plottedPath.Count - 1];
            if (t1.x == t2.x && t1.y == t2.y && t1.z == t2.z)
                return;
        }

        var source = GameObject.Find("MovedTargetSoundEffect").GetComponent<AudioSource>();
        //if (!source.isPlaying)
            source.Play();
    }

    private void PlayConfirmPath() {
        var source = GameObject.Find("ConfirmPathSoundEffect").GetComponent<AudioSource>();
        //if (!source.isPlaying)
            source.Play();
    }

    public void PlotPath(List<Tile> path) {
        if (EqualPath(path, plottedPath))
            return;
        PlayMovedTarget();
        plottedPath = path;

        //Debug.Log("New Path: "+path.Count);

        ClearPath();

        foreach (var tile in path) {
            if (tile == path[path.Count - 1])
                continue;
            AddPathMarker(tile);
        }

        if(path.Count != 0)
            AddTarget(path[path.Count - 1]);
    }

    private bool EqualPath(List<Tile> path, List<Tile> path2) {
        if (path == null || path2 == null || path.Count != path2.Count)
            return false;

        for (int i = 0; i < path.Count; i++) {
            var t1 = path[i];
            var t2 = path2[i];
            if (t1.x != t2.x || t1.y != t2.y || t1.z != t2.z)
                return false;
        }

        return true;
    }

    private void ClearPath() {
        var contianer = GameObject.Find("CursorContainer");
        foreach(Transform obj in contianer.transform) {
            Destroy(obj.gameObject);
        }
    }

    private void AddTarget(Tile tile) {
        GameObject token = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/GridMovement/TargetMarker.prefab");
        AddToken(tile, token);
    }

    private void AddPathMarker(Tile tile) {
        GameObject token = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/GridMovement/MovementToken.prefab");
        AddToken(tile, token);
    }

    private void AddToken(Tile tile, GameObject token) {
        var tokenObj = Instantiate(token);
        var pos = tile.transform.position;
        tokenObj.transform.position = new Vector3(pos.x,pos.y+1,pos.z);
        tokenObj.transform.parent = GameObject.Find("CursorContainer").transform;
    }

    private void RotateTowards(Vector3 target) {
        Vector3 targetDirection = target - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(targetDirection.x, 0, targetDirection.z));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * (speed+2f));
    }

    [TargetRpc]
    public void RpcSetMovementTurn(NetworkConnectionToClient target)
    {
        movementTurn = true;
    }

    [TargetRpc]
    public void RpcSetMoveDestination(NetworkConnectionToClient target) {
        if (path.Count < 1) {
            GetComponent<CharacterGridInfo>().CmdClearMovingTowards();
            return;
        }

        SetMoveDestination(path[0], GetComponent<CharacterGridInfo>());
    }

    public void SetMoveDestination(Tile tile, CharacterGridInfo info) {
        if (tile.IsBlocked(gameObject))
        {
            path.Clear();
            info.CmdClearMovingTowards();
            return;
        }

        info.CmdSetMovingTowards(tile.x, tile.y);
    }

    private void TranslateCharacter(Vector3 target) {
        var step = speed * Time.deltaTime;
        
        
        RotateTowards(target);
        var newPos = Vector3.MoveTowards(transform.position, target, step);
        transform.position = newPos;
    }

    private void EndCharacterMovement(Tile tile, CharacterGridInfo info, Vector3 target) {
        if (Vector3.Distance(transform.position, target) < 0.001f)
        {
            PositionCharacterOnLine(tile);
            path.RemoveAt(0);
            info.CmdClearMovingTowards();

            if (GameManager.Instance.turnBasedMovement)
                path.Clear();
        }
    }

    private void MoveAlongPath()
    {
        var info = GetComponent<CharacterGridInfo>();
        var tile = path[0];
        var target = tile.transform.position;
        target.y += 1;

        SetMoveDestination(tile, info);

        TranslateCharacter(target);

        EndCharacterMovement(tile, info, target);
    }

    private void PositionCharacterOnLine(Tile tile)
    {
        transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y+1f, tile.transform.position.z);
        //character.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        character.standingOnTile = tile;
        GetComponent<CharacterGridInfo>().CmdSetStandingOnTile(tile.x, tile.y);
    }

}