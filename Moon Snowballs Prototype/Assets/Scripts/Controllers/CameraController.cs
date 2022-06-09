using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerBlue;
    [SerializeField] private Transform playerRed;

    [SerializeField] private float maxDistance;
    [SerializeField] private float followingSpeed;
    [SerializeField] private float orthographicSizeChangeSpeed;
    [SerializeField] private float zoomSize;

    bool startZoom;

    float oldSize;

    Camera cam;

    Transform zoomTarget;

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
        Following();

        if (!startZoom)
        {
            Vector2 offset = playerBlue.position - playerRed.position;
            target = GetMiddleBetweenTargets(playerBlue.position, playerRed.position);

            ChangeOrtographicSize(offset);
        }
        else
        {
            target = zoomTarget.position;
            ChangeOrtographicSize(zoomSize);
        }        
    }

    void Following()
    {
        Vector3 newLerpPos = Vector3.Lerp(transform.position, target, followingSpeed * Time.deltaTime);
        newLerpPos.z = transform.position.z;
        transform.position = newLerpPos;
    }

    void ChangeOrtographicSize(Vector3 offset)
    {
        float distance = offset.magnitude;

        if (distance > maxDistance)
        {
            float lerpingSize = Mathf.Lerp(cam.orthographicSize, oldSize + (distance - maxDistance) / 2f, orthographicSizeChangeSpeed * Time.deltaTime);
            cam.orthographicSize = lerpingSize;
        }
    }

    void ChangeOrtographicSize(float size)
    {
        float lerpingSize = Mathf.Lerp(cam.orthographicSize, size, orthographicSizeChangeSpeed * Time.deltaTime);
        cam.orthographicSize = lerpingSize;
    }

    public void StartZoom(Transform target)
    {
        zoomTarget = target;
        startZoom = true;
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
