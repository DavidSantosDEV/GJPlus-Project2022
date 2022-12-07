using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public Transform[] SpawnPoints;
    public FishBehaviour fishPrefab;
    public float fishSpeed=10;

    public float timeForSpawn=1;
    public float TimeForSpawnMax=10;

    private void OnEnable()
    {
        StartCoroutine(SpawnFish());
    }
    private void OnDisable()
    {
        StopCoroutine(SpawnFish());
    }

    private IEnumerator SpawnFish()
    {
        while (isActiveAndEnabled)
        {
            int randomindex = (int)Random.Range(0, SpawnPoints.Length);
            Vector2 pos = SpawnPoints[randomindex].position;
            FishBehaviour fishBehaviour = Instantiate(fishPrefab,pos,Quaternion.identity);
            if (fishBehaviour)
            {
                fishBehaviour.SetDir(pos.x < 0 ? Vector2.right : Vector2.left);
                fishBehaviour.SetSpeed(fishSpeed);
                fishBehaviour.transform.parent = transform;
            }

            yield return new WaitForSeconds(Random.Range(timeForSpawn, TimeForSpawnMax));
        }
        
    }
}
