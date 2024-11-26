using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;

public class GameplayManager : MonoBehaviour
{
    public static List<Tower> towers;
    public static List<Enemy> enemies;

    public GameObject[] enemyPrefabs;

    public EnemyWave[] waves;
    public EnemySpanwer[] spawners;
    int curWave = 0;

    public Vector3[] path1;
    void Start()
    {
        
    }

    void Update()
    {
        if (curWave < waves.Length)
        {
            spawners[waves[curWave].spawnerIdx].SpawnWave(waves[curWave]);
            curWave++;
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
    }
}

[System.Serializable]
public struct EnemyWave
{
    public EnemyType type;
    public int nb;
    public float timeBetweenEach;
    public int spawnerIdx;
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