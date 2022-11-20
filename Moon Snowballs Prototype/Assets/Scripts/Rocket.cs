using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float maxFlyTime;
    [Header("RB Settings")]
    [SerializeField] private float speedChangeForce;
    [SerializeField] private float maxForce;
    [SerializeField] private float maxSpeed;
    [Header("Particle Settings")]
    [SerializeField] private float speedChangeParitcleStartSpeed;
    [SerializeField] private float maxParticleSpeed;

    Rigidbody2D rb;
    ParticleSystem pSystem;

    float currentTime;
    float startForce;
    float pSpeed;
    bool startEngine;
    
    private void Start()
    {
        pSystem = GetComponentInChildren<ParticleSystem>();
        rb = GetComponent<Rigidbody2D>();
        pSystem.Stop();
    }

    private void FixedUpdate()
    {
        if (startEngine)
        {
            Fly();
            ParticleJet();

            currentTime += Time.deltaTime;
            if(currentTime > maxFlyTime)
            {
                startEngine = false;
                ActiveEmission(false);
                pSpeed = 0;
                startForce = 0;
                currentTime = 0;
            }
        }
    }

    void Fly()
    {
        startForce = Mathf.Lerp(startForce, maxForce, speedChangeForce * Time.deltaTime);
        rb.AddForce(transform.up * startForce);
    }

    void ParticleJet()
    {
        pSpeed = Mathf.Lerp(pSpeed, maxParticleSpeed, speedChangeParitcleStartSpeed * Time.deltaTime);
        pSystem.startSpeed = pSpeed;
    }

    void ActiveEmission(bool value)
    {
        var emiss = pSystem.emission;
        emiss.enabled = value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Shell>())
        {
            pSystem.Play();
            ActiveEmission(true);
            startEngine = true;
        }
    }
}
