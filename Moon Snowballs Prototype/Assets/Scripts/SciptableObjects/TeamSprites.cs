using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TeamSprites", menuName = "TeamSprites", order = 52)]
public class TeamSprites : ScriptableObject
{
    public Sprite head;
    public Sprite deadHead;
    public Sprite helmet;
    public Sprite helmetDilapidate;
    public Sprite helmetDestroy;
    public Sprite hand;
    public Sprite body;
    public Sprite ballon;

    public GameObject shell;
    public GameObject mpShell;

    public Gradient trailColor;
}
