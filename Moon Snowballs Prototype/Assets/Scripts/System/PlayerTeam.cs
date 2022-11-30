using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeam : MonoBehaviour
{
    public Team team;

    [SerializeField] private bool isMPPlayer;
    [Header("Body Parts")]
    [SerializeField] private SpriteRenderer headSR;
    [SerializeField] private SpriteRenderer helmetSR;
    [SerializeField] private SpriteRenderer helmetDilapidateSR;
    [SerializeField] private SpriteRenderer helmetDestroySR;
    [SerializeField] private SpriteRenderer handSR;
    [SerializeField] private SpriteRenderer bodySR;
    [SerializeField] private SpriteRenderer ballonSR;
    [SerializeField] private TrailRenderer trail;

    [Header("Team Sprite Data")]
    [SerializeField] private TeamSprites blueTeam;
    [SerializeField] private TeamSprites redTeam;

    [HideInInspector] public GameObject shellPref;

    private void OnValidate()
    {
        if (blueTeam & redTeam)
            SetSprites();
    }

    public void SetSprites()
    {
        if (team == Team.Blue)
        {
            headSR.sprite = blueTeam.head;
            helmetSR.sprite = blueTeam.helmet;
            helmetDilapidateSR.sprite = blueTeam.helmetDilapidate;
            helmetDestroySR.sprite = blueTeam.helmetDestroy;
            handSR.sprite = blueTeam.hand;
            bodySR.sprite = blueTeam.body;
            ballonSR.sprite = blueTeam.ballon;
            transform.localScale = new Vector3(1, 1, 1);
            if (isMPPlayer) shellPref = blueTeam.mpShell;
            else shellPref = blueTeam.shell;
            trail.colorGradient = blueTeam.trailColor;
        }
        else
        {
            headSR.sprite = redTeam.head;
            helmetSR.sprite = redTeam.helmet;
            helmetDilapidateSR.sprite = redTeam.helmetDilapidate;
            helmetDestroySR.sprite = redTeam.helmetDestroy;
            handSR.sprite = redTeam.hand;
            bodySR.sprite = redTeam.body;
            ballonSR.sprite = redTeam.ballon;
            transform.localScale = new Vector3(-1, 1,1);
            if (isMPPlayer) shellPref = redTeam.mpShell;
            else shellPref = redTeam.shell;
            trail.colorGradient= redTeam.trailColor;
        }
        ChangeLayer();
    }

    void ChangeLayer()
    {
        if (team == Team.Blue)
        {
            gameObject.layer = 11;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.layer = 11;
            }
        }
        else
        {
            gameObject.layer = 12;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.layer = 12;
            }
        }
    }
}

public enum Team
{
    Blue,
    Red
}
