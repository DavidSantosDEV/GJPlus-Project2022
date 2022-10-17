using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public Transform[] SpawnPoints;
    public GameObject fishPrefab;
    public float fishSpeed=10;

    public float timeForSpawn=1;
    public float TimeForSpawnMax=10;

    private void OnEnable()
    {
        StartCoroutine(SpawnFish());
    }

    private IEnumerator SpawnFish()
    {
        while (isActiveAndEnabled)
        {
            int randomindex = (int)Random.Range(0, SpawnPoints.Length);
            Instantiate(fishPrefab,SpawnPoints[randomindex]);
            yield return new WaitForSeconds(Random.Range(timeForSpawn, TimeForSpawnMax));
        }
        
    }
}
