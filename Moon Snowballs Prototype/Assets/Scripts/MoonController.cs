using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonController : MonoBehaviour
{
    public float force;

    Vector2 direction;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Rigidbody2D>())
        {
            direction = transform.position - collision.transform.position;

            Rigidbody2D rb = collision.GetComponentInParent<Rigidbody2D>();

            rb.AddForce(direction * force, ForceMode2D.Force);
        }

        if (collision.gameObject.GetComponent<Rigidbody2D>())
        {
            direction = transform.position - collision.transform.position;

            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            rb.AddForce(direction * force, ForceMode2D.Force);
        }
    }
}
