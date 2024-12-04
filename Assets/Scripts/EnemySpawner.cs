using DG.Tweening;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpanwer : MonoBehaviour
{
    GameplayManager gameplayManager;
    public Vector3 spawnOffsetDirection; // either (1,0,0) or (0,0,1)
    public int id;

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

        WaitForSeconds delay = new WaitForSeconds(wave.timeBetweenEach);
        for (int i = 0; i < wave.nb; i++)
        {
            float spawnPosOffset = Random.Range(-1.0f, 1.0f);

            Vector3 spawnPos = transform.position + spawnOffsetDirection * spawnPosOffset;
            GameObject newObj = Instantiate(prefab, spawnPos, Quaternion.identity);
            Enemy newE = newE = newObj.transform.GetChild(0).GetComponent<Enemy>();
            if (prefab.CompareTag("flyingE"))
            {
                Vector3 flyingPos = newE.transform.position; //newObj.transform.GetChild(0).position;
                flyingPos.y += Random.Range(3f, 4f);
                newE.transform/*.GetChild(0)*/.position = flyingPos;
            }
            
            if (wave.type == EnemyType.DroneSpawnerBoss || wave.type == EnemyType.WalkerBoss)
            {
                // boss warning
            }
            
            Vector3[] ogPath;
            if (wave.spawnerIdx == 0)
            {
                ogPath = gameplayManager.path1;
                newE.pathId = 0;
            }
            else
            {
                ogPath = gameplayManager.path2;
                newE.pathId = 1;
            }

            Vector3[] randomPath = new Vector3[ogPath.Length];

            for(int j = 0; j < ogPath.Length - 1; j++)
            {
                randomPath[j] = ogPath[j] + new Vector3(Random.Range(-4.5f, 4.5f), 0, Random.Range(-4.0f, 4.0f));
            }

            randomPath[randomPath.Length - 1] = ogPath[ogPath.Length - 1];

            newE.path = randomPath;
            newE.navCor = newE.StartCoroutine(newE.MoveAlongPath());

            yield return delay;
        }
    }

    public void SpawnWave(EnemyWave wave)
    {
        StartCoroutine(SpawnWaveLoop(wave));
    }
}
