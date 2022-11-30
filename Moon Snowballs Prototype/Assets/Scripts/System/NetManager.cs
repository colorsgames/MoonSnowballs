using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class NetManager : NetworkRoomManager
{
    Button mpButton;

    public void OnCreateRoomPlayer(NetworkConnectionToClient conn, PosMessage mess)
    {
        GameObject go = Instantiate(roomPlayerPrefab, mess.position, Quaternion.identity).gameObject;
        NetworkServer.AddPlayerForConnection(conn, go);
    }

    public override void OnRoomStartServer()
    {
        base.OnRoomStartServer();
        NetworkServer.RegisterHandler<PosMessage>(OnCreateRoomPlayer);
    }

    public void SpawnRoomPlayer()
    {
        PosMessage mess = new PosMessage();
        mess.position = new Vector2(-6, 12.21f);
        NetworkClient.Send(mess);
    }

    public override void OnRoomClientConnect()
    {
        base.OnRoomClientConnect();
        SpawnRoomPlayer();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        StopHost();
    }

    void FindButton()
    {
        if (GameObject.Find("MultiplayerButton"))
        {
            mpButton = GameObject.Find("MultiplayerButton").GetComponent<Button>();
        }
    }
}

public struct PosMessage : NetworkMessage
{
    public Vector2 position;
}
