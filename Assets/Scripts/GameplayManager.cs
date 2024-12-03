using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameplayManager : MonoBehaviour
{
    public static List<Tower> towers;
    public static List<Enemy> enemies;

    public GameObject[] enemyPrefabs;
    public GameObject[] towerPrefabs;

    // public EnemyWave[] waves;

    [System.Serializable]
    public struct Round
    {
        public Round(EnemyWave[] waves)
        {
            this.waves = waves;
        }
        public EnemyWave[] waves;
    };

    public Round[] rounds;

    public EnemySpanwer[] spawners;
    int curWaveIdx = 0;

    public Vector3[] path1;
    public Vector3[] path2;

    public TowerTile currentTowerTile;
    GameObject cube;

    List<FixedTower> placedTowers;

    GameObject overlay;
    TMP_Text moneyText;
    TMP_Text roundText;


    public AudioClip[] booms;
    Player p;

    void Start()
    {
        rounds = new Round[3]{
            new Round(new EnemyWave[] {new EnemyWave(EnemyType.E1, 3, 0.5f, 0, 1.0f), new EnemyWave(EnemyType.E2, 3, 0.5f, 0, 1.0f)}),
            new Round(new EnemyWave[] {new EnemyWave(EnemyType.E1, 2, 0.5f, 1, 0.0f), new EnemyWave(EnemyType.E1, 3, 0.5f, 0, 0.0f)}),
            new Round(new EnemyWave[] {new EnemyWave(EnemyType.E3, 2, 0.6f, 0, 0.0f), new EnemyWave(EnemyType.E3, 2, 0.6f, 1, 0.0f), new EnemyWave(EnemyType.E2, 2, 0.6f, 0, 0.0f), new EnemyWave(EnemyType.E2, 2, 0.6f, 1, 0.0f)}),
            /*new Round(new EnemyWave[] {}),
            new Round(new EnemyWave[] {}),
            new Round(new EnemyWave[] {}),
            new Round(new EnemyWave[] {}),
            new Round(new EnemyWave[] {}),
            new Round(new EnemyWave[] {}),
            new Round(new EnemyWave[] {})
*/        };

        rounds = new Round[1]
        {
            new Round(new EnemyWave[]{new EnemyWave(EnemyType.E2, 10, 5.5f, 0, 0.0f) })
        };
        p = GameObject.FindWithTag("player").GetComponent<Player>();
        overlay = GameObject.FindWithTag("ui_overlay");
        overlay.SetActive(true);

        roundText = GameObject.FindWithTag("RoundText").GetComponent<TMP_Text>();
        roundText.text = "Round: 0";

        placedTowers = new List<FixedTower> ();
        Enemy.curId = 0;
        cube = transform.GetChild(0).gameObject;
        cube.SetActive(false);
        StartCoroutine(IterateRounds());
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

    bool roundDone = false;
    IEnumerator IterateRounds()
    {
        WaitForSeconds afterRoundDelay = new WaitForSeconds(10.0f);
        for (int i = 0; i < rounds.Length; i++)
        {
            roundText.text = "Round: " + (i + 1);
            roundDone = false;
            Round round = rounds[i];
            StartCoroutine(IterateWaves(round));
            while (!roundDone) yield return null;
            yield return afterRoundDelay;
        }
    }
    IEnumerator IterateWaves(Round round)
    {
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < round.waves.Length; i++)
        {
            EnemyWave curWave = round.waves[curWaveIdx];
            spawners[curWave.spawnerIdx].SpawnWave(curWave);
            curWaveIdx++;
            yield return new WaitForSeconds(curWave.afterWaveDelay);
        }
        roundDone = true;
        curWaveIdx = 0;
    }
    public void Lose()
    {
        DOTween.KillAll();
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

    public AudioClip GetRandomBoom()
    {
        return booms[Random.Range(0, booms.Length)];
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

    public EnemyWave(EnemyType t, int nbE, float tb = 0.25f, int spawnerId = 0, float timeAfter = 1.0f)
    {
        type = t;
        nb = nbE;
        timeBetweenEach = tb;
        spawnerIdx = spawnerId;
        afterWaveDelay = timeAfter;
    }
};

public enum EnemyType
{
    E1 = 0,
    E2,
    E3,
    Boss1,
    Boss2
}