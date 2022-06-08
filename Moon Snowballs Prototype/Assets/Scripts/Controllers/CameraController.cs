using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerBlue;
    [SerializeField] private Transform playerRed;

    [SerializeField] private float minDistance;
    [SerializeField] private float followingSpeed;
    [SerializeField] private float orthographicSizeChangeSpeed;


    float oldSize;

    Camera cam;

    Vector3 target;

    private void Start()
    {
        cam = GetComponent<Camera>();
        oldSize = cam.orthographicSize;

        Vector2 offset = playerBlue.position - playerRed.position;
        target = GetMiddleBetweenTargets(playerBlue.position, playerRed.position);
        target.z = transform.position.z;
        transform.position = target;
    }

    private void FixedUpdate()
    {
        Vector2 offset = playerBlue.position - playerRed.position;
        target = GetMiddleBetweenTargets(playerBlue.position, playerRed.position);

        Vector3 newLerpPos = Vector3.Lerp(transform.position, target, followingSpeed * Time.deltaTime);
        newLerpPos.z = transform.position.z;
        transform.position = newLerpPos;

        ChangeOrtographicSize(offset);
    }

    void ChangeOrtographicSize(Vector3 offset)
    {
        float distance = offset.magnitude;

        if(distance > minDistance)
        {
            float lerpingSize = Mathf.Lerp(cam.orthographicSize, oldSize + (distance - minDistance), orthographicSizeChangeSpeed * Time.deltaTime);
            cam.orthographicSize = lerpingSize;
        }
    }

    private Vector3 GetMiddleBetweenTargets(Vector3 first, Vector3 second)
    {
        Vector3 offset = first - second;
        float distance = offset.magnitude;
        Vector3 middleTarget = second + offset.normalized * distance / 2;
        return middleTarget;
    }

    private void OnDrawGizmos()
    {
        Vector3 offset = playerRed.position - playerBlue.position;
        Gizmos.DrawSphere(GetMiddleBetweenTargets(playerBlue.position, playerRed.position), 0.5f);
        Gizmos.DrawRay(playerBlue.position, offset.normalized * offset.magnitude);
    }
}
