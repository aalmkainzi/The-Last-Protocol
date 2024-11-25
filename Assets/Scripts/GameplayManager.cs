using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class GameplayManager : MonoBehaviour
{
    public static List<Tower> towers;
    public static List<Enemy> enemies;

    public GameObject[] enemyPrefabs;

    public EnemyWave[] waves;
    public EnemySpanwer[] spawners;
    int curWave = 0;

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