using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cometPrefab;

    [SerializeField] private int cometSpawnCount;
    [SerializeField] private float changePosDelay;

    private CometController[] comets;

    private void Start()
    {
        comets = new CometController[cometSpawnCount];

        for (int i = 0; i < comets.Length; i++)
        {
            comets[i] = Instantiate(cometPrefab).GetComponent<CometController>();
        }

        StartCoroutine(ChangePos(changePosDelay));
    }

    IEnumerator ChangePos(float time)
    {
        while (true)
        {
            for (int i = 0; i < comets.Length; i++)
            {
                comets[i].gameObject.SetActive(false);
                Vector3 position = new Vector3(Random.Range(-200, 200), Random.Range(-200, 200), 10);
                comets[i].transform.position = position;
                comets[i].gameObject.SetActive(true);
                comets[i].ChangeParams();
            }
            
            yield return new WaitForSeconds(time);
        }
    }

    void SetActiveComets(bool value)
    {
        foreach (CometController comet in comets)
        {
            comet.gameObject.SetActive(value);
        }
    }
}
