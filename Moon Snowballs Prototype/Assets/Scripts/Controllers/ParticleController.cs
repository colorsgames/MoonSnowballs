using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public float lifeTime;

    new ParticleSystem particleSystem;

    float curretTime;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        Destroy(this.gameObject, lifeTime + 1);
    }

    private void Update()
    {
        curretTime += Time.deltaTime;
        if (curretTime >= lifeTime)
        {
            var emis = particleSystem.emission;
            emis.enabled = false;
        }
    }
}
