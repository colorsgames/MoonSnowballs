using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player_o;
    public Transform player_1;
    public Transform moon;

    public float minDistance;

    float distance_0;
    float distance_1;

    float oldSize;

    Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        oldSize = cam.orthographicSize;
    }

    private void Update()
    {
        distance_0 = Vector3.Distance(player_o.position, moon.position);
        distance_1 = Vector3.Distance(player_1.position, moon.position);

        if(distance_0 > minDistance & distance_0 > distance_1)
        {
            cam.orthographicSize = oldSize + (distance_0 - minDistance);
        }
        if (distance_1 > minDistance & distance_1 > distance_0)
        {
            cam.orthographicSize = oldSize + (distance_1 - minDistance);
        }
    }
}
