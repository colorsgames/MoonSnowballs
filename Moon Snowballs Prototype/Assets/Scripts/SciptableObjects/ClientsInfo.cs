using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New ClientInfo", menuName = "SupportNetworkGame", order = 53)]
public class ClientsInfo : ScriptableObject
{
    public bool isHost;
    public bool isClient;
}
