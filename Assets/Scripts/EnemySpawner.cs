using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

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
        Enemy prefabE = prefab.GetComponent<Enemy>();
        WaitForSeconds delay = new WaitForSeconds(wave.timeBetweenEach);
        for(int i = 0; i < wave.nb; i++)
        {
            float spawnPosOffset = Random.Range(-1.0f, 1.0f);
            Vector3 spawnPos = transform.position + spawnOffsetDirection * spawnPosOffset;
            GameObject newObj = Instantiate(prefab, spawnPos, Quaternion.identity);
            Enemy newE = newObj.GetComponent<Enemy>();
            if (prefabE.flying)
            {
                Vector3 flyingPos = newObj.transform.GetChild(0).position;
                flyingPos.y += Random.Range(3f, 4f);
                newObj.transform.GetChild(0).position = flyingPos;
            }

            Vector3[] ogPath = gameplayManager.path1;
            Vector3[] randomPath = new Vector3[ogPath.Length];

            for(int j = 0; j < ogPath.Length; j++)
            {
                randomPath[j] = ogPath[j] + new Vector3(Random.Range(-4.5f, 4.5f), 0, Random.Range(-4.0f, 4.0f));
            }

            newE.path = randomPath;
            newE.agent = newObj.GetComponent<NavMeshAgent>();
            StartCoroutine(newE.MoveAlongPath());

            yield return delay;
        }
    }

    public void SpawnWave(EnemyWave wave)
    {
        StartCoroutine(SpawnWaveLoop(wave));
    }
}
