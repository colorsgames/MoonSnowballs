using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public GameObject glassParticlePrefab;

    public Sprite normalHead;
    public Sprite deadHead;

    SpriteRenderer SpriteRenderer;

    public CircleCollider2D[] colliders2D;
    public GameObject[] helmets;

    public bool destroy;
    public float health = 100;

    public int index;

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
        SpriteRenderer = GetComponent<SpriteRenderer>();

        oldHealth = health;
    }

    private void Update()
    {
        helmets[index].SetActive(true);
        if(index > 0)
        {
            helmets[index - 1].SetActive(false);
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

        if(health == oldHealth / 2)
        {
            index = 1;
        }

        if(health <= 0)
        {
            Instantiate(glassParticlePrefab, transform.position, Quaternion.identity);
            SpriteRenderer.sprite = deadHead;
            index = 2;
            destroy = true;
            foreach (CircleCollider2D item in colliders2D)
            {
                item.enabled = false;
            }
            _destroyName = gameObject.name;
        }
    }
}
