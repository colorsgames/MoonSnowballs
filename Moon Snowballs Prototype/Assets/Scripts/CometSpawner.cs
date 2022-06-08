using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometSpawner : MonoBehaviour
{
    public GameObject cometPrefab;

    public float spawnDelay;

    float curretTime;

    private void Update()
    {
        curretTime += Time.deltaTime;
        if(curretTime >= spawnDelay)
        {
            Vector3 position = new Vector3(Random.Range(-200, 200), Random.Range(-200, 200), 10);
            Instantiate(cometPrefab, position, Quaternion.identity);
            curretTime = 0;
        }
    }
}
