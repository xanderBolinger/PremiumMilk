using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    
    public static GameManager Instance;

    [SyncVar]
    public bool gridMovement;
    [SyncVar]
    public bool playGridMovment;
    [SyncVar]
    public bool turnPaused;


    public void Start()
    {
        Instance = this;
    }

    public void Update()
    {
        
    }

    public bool PlayGridMovement() {
        return gridMovement && playGridMovment;
    }





}
