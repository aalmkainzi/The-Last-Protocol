using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using static UnityEditor.PlayerSettings;

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
        GameObject prefab = gameplayManager.enemyPrefabs[(int)wave.type];
        Enemy prefabE = prefab.GetComponent<Enemy>();
        float delay = wave.timeBetweenEach;
        for(int i = 0; i < wave.nb; i++)
        {
            float spawnPosOffset = Random.Range(-1.0f, 1.0f);
            Vector3 spawnPos = transform.position + spawnOffsetDirection * spawnPosOffset;
            GameObject newE = Instantiate(prefab, spawnPos, Quaternion.identity);
            Debug.Log("Spawned AT: " + spawnPos);
            if (prefabE.flying)
            {
                Vector3 flyingPos = newE.transform.GetChild(0).position;
                flyingPos.y += Random.Range(3f, 4f);
                newE.transform.GetChild(0).position = flyingPos;
                Debug.Log("FlyingPos : " + newE.transform.GetChild(0).position);
            }

            
            yield return delay;
        }
    }

    public void SpawnWave(EnemyWave wave)
    {
        StartCoroutine(SpawnWaveLoop(wave));
    }
}
