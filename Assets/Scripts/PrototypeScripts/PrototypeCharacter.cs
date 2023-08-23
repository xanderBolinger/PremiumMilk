using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeCharacter : NetworkBehaviour
{
    [SyncVar]
    public bool syncVar;

    public bool futureSyncVarValue;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("A new prototype character has connected");        
    }

    public void SetSyncVar(bool syncVar) { 
        this.syncVar = syncVar;
    } 

    [Command]
    public void CmdSetSyncVar(bool syncVar) { 
        this.syncVar = syncVar;
    }

    [TargetRpc]
    public void RpcTargetSetSyncVar(NetworkConnectionToClient target, bool syncVar) {
        this.syncVar = syncVar;
    }

    [TargetRpc]
    public void RpcSetSyncVar(bool syncVar)
    {
        this.syncVar = syncVar;
    }

    [ClientRpc]
    public void ClientRpcSetSyncVar(bool syncVar)
    {
        this.syncVar = syncVar;
    }

}
