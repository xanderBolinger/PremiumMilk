using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartServer : MonoBehaviour
{

    void Start()
    {

        Debug.Log("Start Sever");
        NetworkManager.singleton.StartHost();
        Debug.Log("Pass Start Server");
    }

}
