using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [HideInInspector] public bool destroy;

    [SerializeField] private GameObject glassParticlePrefab;

    [SerializeField] private Sprite normalHead;
    [SerializeField] private Sprite deadHead;

    [SerializeField] private CircleCollider2D[] colliders2D;
    [SerializeField] private GameObject[] helmets;

    [SerializeField] private float health = 100;

    [SerializeField] private int indexHelmet;

    PlayerTeam team;

    SpriteRenderer SpriteRenderer;

    float oldHealth;
    string _destroyName;

    public string destroyName
    {
        get
        {
            return (_destroyName);
        }
        set
        {
            _destroyName = value;
        }
    }

    private void Start()
    {
        team = GetComponentInParent<PlayerTeam>();
        ChangeName();
        GameManager.instance.AddHead(this);
        SpriteRenderer = GetComponent<SpriteRenderer>();

        oldHealth = health;
    }

    private void Update()
    {
        helmets[indexHelmet].SetActive(true);
        if (indexHelmet > 0)
        {
            helmets[indexHelmet - 1].SetActive(false);
        }

        if (!destroy)
        {
            SpriteRenderer.sprite = normalHead;
            foreach (CircleCollider2D item in colliders2D)
            {
                item.enabled = true;
            }
        }
    }

    public void Damage(float _damage)
    {
        health -= _damage;

        if (health == oldHealth / 2)
        {
            indexHelmet = 1;
        }

        if (health <= 0)
        {
            Destroy();
        }
    }

    void ChangeName()
    {
        if(team.team == Team.Blue)
        {
            gameObject.name = "Head_0";
        }
        else
        {
            gameObject.name = "Head_1";
        }
    }

    void Destroy()
    {
        Instantiate(glassParticlePrefab, transform.position, Quaternion.identity);
        SpriteRenderer.sprite = deadHead;
        indexHelmet = 2;
        destroy = true;
        foreach (CircleCollider2D item in colliders2D)
        {
            item.enabled = false;
        }
        _destroyName = gameObject.name;
    }
}
