using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
// using DG.Tweening;
using PrimeTween;

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

    public List<FixedTower> placedTowers;

    GameObject overlay;
    TMP_Text moneyText;
    TMP_Text roundText;


    public AudioClip[] booms;
    Player p;

    public AudioClip[] enemyDamageSounds;

    public GameObject towerUI;

    void Start()
    {
        PrimeTweenConfig.SetTweensCapacity(1000);

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
            new Round(new EnemyWave[]{new EnemyWave(EnemyType.E1, 1000, 0.25f, 0, 0.0f) })
        };

        p = GameObject.FindWithTag("player").GetComponent<Player>();
        overlay = GameObject.FindWithTag("ui_overlay");
        overlay.SetActive(true);

        towerUI = GameObject.FindWithTag("ui_tower");
        towerUI.SetActive(false);

        roundText = GameObject.FindWithTag("RoundText").GetComponent<TMP_Text>();
        roundText.text = "Round: 0";

        moneyText = GameObject.FindWithTag("MoneyText").GetComponent<TMP_Text>();
        moneyText.text = p.money + "";

        placedTowers = new List<FixedTower> ();
        Enemy.curId = 0;
        cube = transform.GetChild(0).gameObject;
        cube.SetActive(false);

        StartCoroutine(IterateRounds());
    }

    public void GetMoney(int money)
    {
        p.money += money;
    }

    void Update()
    {
        if(currentTowerTile != null)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                EnableTowerUI();
            }
        }

        moneyText.text = p.money + "";
    }

    public void EnableTowerUI()
    {
        towerUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void DisableTowerUI()
    {
        towerUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SelectTower(int towerTy)
    {
        Tower towerPrefab = towerPrefabs[(int)towerTy].GetComponent<Tower>();
        if (p.money >= towerPrefab.price)
        {
            p.money -= towerPrefab.price;
            currentTowerTile.PlaceTower(towerPrefab);
        }
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
        // DOTween.KillAll();
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

[System.Serializable]
public enum TowerType
{
    Bomb,
    Generator,
    Turret,
    Rail
};