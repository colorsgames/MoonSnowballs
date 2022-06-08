using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationMoon : MonoBehaviour
{
    public float rotationSpeed;

    private void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
