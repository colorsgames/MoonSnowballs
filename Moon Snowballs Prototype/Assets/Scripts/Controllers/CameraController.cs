using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [SerializeField] private List<Transform> players;

    [SerializeField] private float maxDistance;
    [SerializeField] private float followingSpeed;
    [SerializeField] private float orthographicSizeChangeSpeed;
    [SerializeField] private float zoomSize;

    bool startZoom;

    float oldSize;

    Camera cam;

    Transform zoomTarget;

    Vector3 target;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        cam = GetComponent<Camera>();
        oldSize = cam.orthographicSize;
        if (players.Count == 2)
        {
            Vector2 offset = players[0].position - players[1].position;
            target = GetMiddleBetweenTargets(players[0].position, players[1].position);
            target.z = transform.position.z;
            transform.position = target;
        }
    }

    private void FixedUpdate()
    {
        Following();

        if (!startZoom)
        {
            if (players.Count == 2)
            {
                Vector2 offset = players[0].position - players[1].position;
                target = GetMiddleBetweenTargets(players[0].position, players[1].position);

                ChangeOrtographicSize(offset);
            }
            else if(players.Count == 1)
            {
                target = players[0].position;
            }
        }
        else
        {
            target = zoomTarget.position;
            ChangeOrtographicSize(zoomSize);
        }        
    }

    public void AddPlayer(Transform transform)
    {
        players.Add(transform);
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
}
