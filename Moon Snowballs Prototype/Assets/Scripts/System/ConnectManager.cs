using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ConnectManager : MonoBehaviour
{
    [SerializeField] private ClientsInfo cl;

    NetworkRoomManager roomManager;

    private void Start()
    {
        roomManager = FindObjectOfType<NetworkRoomManager>();
    }

    public void Connection()
    {

    }

    public void CreateServer()
    {
        cl.isHost = true;
        cl.isClient = false;
        roomManager.StartHost();
    }

    public void JoinServer()
    {
        cl.isClient = true;
        cl.isHost = false;
        roomManager.networkAddress = "localhost";
        roomManager.StartClient();
    }
}
