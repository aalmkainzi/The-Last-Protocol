using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

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

    List<FixedTower> placedTowers;

    GameObject overlay;
    TMP_Text moneyText;


    public AudioClip[] booms;
    Player p;

    void Start()
    {
        p = GameObject.FindWithTag("player").GetComponent<Player>();
        overlay = GameObject.FindWithTag("ui_overlay");
        overlay.SetActive(true);
        placedTowers = new List<FixedTower> ();
        Enemy.curId = 0;
        cube = transform.GetChild(0).gameObject;
        cube.SetActive(false);
        StartCoroutine(IterateWaves());
    }

    void Update()
    {
        if(currentTowerTile != null)
        {
            cube.SetActive(true);
            cube.transform.position = currentTowerTile.transform.position;
            GameObject newTower = null;
            if (Input.GetKeyDown(KeyCode.E))
            {
                // open towers menu
                newTower = currentTowerTile.PlaceTower(towerPrefabs[0].GetComponent<Tower>());
            }
            else if(Input.GetKeyDown(KeyCode.R))
            {
                newTower = currentTowerTile.PlaceTower(towerPrefabs[3].GetComponent<Tower>());
            }
            if(newTower != null)
            {
                placedTowers.Add(newTower.GetComponent<FixedTower>());
            }
        }
        else
        {
            cube.SetActive(false);
        }
    }

    public void SelectTower()
    {
        moneyText.text = p.money + "";
    }

    public void RemoveEnemyFromAllTowers(Enemy e)
    {
        for(int i = 0; i < placedTowers.Count; i++)
        {
            placedTowers[i].enemiesInRange.Remove(e);
        }
    }

    IEnumerator IterateWaves()
    {
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < waves.Length; i++)
        {
            EnemyWave curWave = waves[curWaveIdx];
            spawners[curWave.spawnerIdx].SpawnWave(curWave);
            curWaveIdx++;
            yield return new WaitForSeconds(curWave.afterWaveDelay);
        }
    }
    public void Lose()
    {
        SceneManager.LoadScene("MainMenu");
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
    Boss1,
    Boss2
}