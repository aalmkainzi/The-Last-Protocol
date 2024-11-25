using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class EnemySpanwer : MonoBehaviour
{
    GameplayManager gameplayManager;
    public Vector3 spawnOffsetDirection; // either (1,0,0) or (0,0,1)
    
    void Start()
    {
        gameplayManager = GameObject.FindWithTag("gameplayManager").GetComponent<GameplayManager>();
    }

    void Update()
    {
        
    }

    IEnumerator SpawnWaveLoop(EnemyWave wave)
    {
        float delay = wave.timeBetweenEach;
        for(int i = 0; i < wave.nb; i++)
        {
            float spawnPosOffset = Random.Range(-1.0f, 1.0f);
            GameObject newE = Instantiate(gameplayManager.enemyPrefabs[(int) wave.type], transform.position + spawnOffsetDirection * spawnPosOffset, Quaternion.identity);
            yield return delay;
        }
    }

    public void SpawnWave(EnemyWave wave)
    {
        StartCoroutine(SpawnWaveLoop(wave));
    }
}
