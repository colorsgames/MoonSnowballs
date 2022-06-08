using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometController : MonoBehaviour
{
    public TrailRenderer trailRenderer;

    private GameObject moon;

    float xSpeed;
    float ySpeed;

    private void Start()
    {
        moon = GameObject.Find("Moon");
    }

    private void Update()
    {
        Vector2 position = transform.position;
        position.x += xSpeed * Time.deltaTime;
        position.y += ySpeed * Time.deltaTime;
        transform.position = position;
    }

    public void ChangeParams()
    {
        xSpeed = Random.Range(-10, 10);
        ySpeed = Random.Range(6, -6);
        float randomValue = Random.Range(0.05f, 0.2f);
        transform.localScale = new Vector3(randomValue, randomValue, randomValue);
        trailRenderer.startWidth = randomValue * 5;
    }
}
