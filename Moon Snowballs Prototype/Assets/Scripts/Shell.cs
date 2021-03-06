using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public GameObject particlePrefab;

    public float lifeTime = 10;
    public float damage;

    private void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(particlePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<HealthController>())
        {
            collision.GetComponent<HealthController>().Damage(damage);
        }
    }
}
