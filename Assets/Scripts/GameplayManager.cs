using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.Collections;

public class GameplayManager : MonoBehaviour
{
    public static List<Tower> towers;
    public static List<Enemy> enemies;

    public GameObject[] enemyPrefabs;
    public GameObject[] towerPrefabs;

    public EnemyWave[] waves;
    public EnemySpanwer[] spawners;
    int curWaveIdx = 0;

    public Vector3[] path1;
    public Vector3[] path2;

    public TowerTile currentTowerTile;
    GameObject cube;

    void Start()
    {
        Enemy.curId = 0;
        cube = transform.GetChild(0).gameObject;
        cube.SetActive(false);
    }

    void Update()
    {
        if(currentTowerTile != null)
        {
            cube.SetActive(true);
            cube.transform.position = currentTowerTile.transform.position;
            if (Input.GetKeyDown(KeyCode.E))
            {
                // open towers menu
                currentTowerTile.PlaceTower(towerPrefabs[0].GetComponent<Tower>());
            }
        }
        else
        {
            cube.SetActive(false);
        }

        if(curWaveIdx < waves.Length)
        {
            EnemyWave curWave = waves[curWaveIdx];
            spawners[curWave.spawnerIdx].SpawnWave(curWave);
            curWaveIdx++;
        }
    }

    public void Lose()
    {
        // TODO impl
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        if (path1 == null || path1.Length == 0) return;

        for (int i = 0; i < path1.Length - 1; i++)
        {
            Gizmos.DrawLine(path1[i], path1[i + 1]);
        }

        Gizmos.color = Color.green;

        if (path2 == null || path2.Length == 0) return;

        for (int i = 0; i < path2.Length - 1; i++)
        {
            Gizmos.DrawLine(path2[i], path2[i + 1]);
        }
    }
}

[System.Serializable]
public struct EnemyWave
{
    public EnemyType type;
    public int nb;
    public float timeBetweenEach;
    public int spawnerIdx;
    public float afterWaveDelay;
};

public enum EnemyType
{
    E1 = 0,
    E2,
    E3,
    E4,
    Boss1,
    Boss2
}