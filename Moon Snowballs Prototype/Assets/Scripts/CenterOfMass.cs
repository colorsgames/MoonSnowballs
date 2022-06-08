using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterOfMass : MonoBehaviour
{
    public Transform centerOfMass;

    Rigidbody2D rigidbody2d;

    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();

        rigidbody2d.centerOfMass = centerOfMass.localPosition;
    }
}
