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
    public static GameplayManager instance;

    public List<Enemy> enemies;

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

    [SerializeField] private AudioClip[] enemyDamageSounds;

    public GameObject towerUI;
    public GameObject upgradeUI;
    public DefeatedMenuController defeatedMenuController;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        PrimeTweenConfig.SetTweensCapacity(1000);

        enemies = new();

        rounds = new Round[5]
        {
            new Round(new EnemyWave[] {new EnemyWave(EnemyType.SmallDrone, 3, 0.5f, 0, 1.0f), new EnemyWave(EnemyType.Crawler, 3, 0.5f, 0, 1.0f)}),
            new Round(new EnemyWave[] {new EnemyWave(EnemyType.SmallDrone, 2, 0.5f, 1, 0.0f), new EnemyWave(EnemyType.SmallDrone, 3, 0.5f, 0, 0.0f)}),
            new Round(new EnemyWave[] {new EnemyWave(EnemyType.BigDrone, 2, 0.6f, 0, 0.0f), new EnemyWave(EnemyType.BigDrone, 2, 0.6f, 1, 0.0f), new EnemyWave(EnemyType.Crawler, 2, 0.6f, 0, 0.0f), new EnemyWave(EnemyType.Crawler, 2, 0.6f, 1, 0.0f)}),
            new Round(new EnemyWave[] {new EnemyWave(EnemyType.BigDrone, 2, 0.6f, 0, 0.0f), new EnemyWave(EnemyType.BigDrone, 2, 0.6f, 1, 0.0f), new EnemyWave(EnemyType.Crawler, 2, 0.6f, 0, 0.0f), new EnemyWave(EnemyType.Crawler, 2, 0.6f, 1, 0.0f)}),
            new Round(new EnemyWave[] {new EnemyWave(EnemyType.DroneSpawnerBoss, 1, 0.6f, 0, 0.0f), new EnemyWave(EnemyType.DroneSpawnerBoss, 2, 0.6f, 1, 0.0f), new EnemyWave(EnemyType.Crawler, 2, 0.6f, 0, 0.0f), new EnemyWave(EnemyType.Crawler, 2, 0.6f, 1, 0.0f)}),
            //new Round(new EnemyWave[] {}),
            //new Round(new EnemyWave[] {}),
            //new Round(new EnemyWave[] {}),
            //new Round(new EnemyWave[] {}),
            //new Round(new EnemyWave[] {})
        };

        //rounds = new Round[1]
        //{
        //    new Round(new EnemyWave[]{
        //        new EnemyWave(EnemyType.SmallDrone, 10, 0.65f, 0, 6.5f),
        //        new EnemyWave(EnemyType.Crawler, 5, 0.6f, 0, 4f),
        //        new EnemyWave(EnemyType.BigDrone, 3, 2.25f, 0, 3.0f),
        //        new EnemyWave(EnemyType.DroneSpawnerBoss, 1, 0.25f, 0, 2.0f),
        //        new EnemyWave(EnemyType.SmallDrone, 10, 0.75f, 1, 6.5f),
        //        new EnemyWave(EnemyType.WalkerBoss, 1, 0.6f, 0, 3.0f),
        //        new EnemyWave(EnemyType.Stealth, 1, 0, 0, 2.0f),
        //        new EnemyWave(EnemyType.Stealth, 1, 0, 1, 0.0f),

        //    })
        //};

        //rounds = new Round[1]
        //{
        //    new Round(new EnemyWave[]{
        //        new EnemyWave(EnemyType.SmallDrone, 50, 1.75f, 0, 0.0f),
        //    })
        //};

        GameObject.Find("BombPrice").GetComponent<TMP_Text>().text = towerPrefabs[0].GetComponent<Tower>().price + "";
        GameObject.Find("GeneratorPrice").GetComponent<TMP_Text>().text = towerPrefabs[1].GetComponent<Tower>().price + "";
        GameObject.Find("TurretPrice").GetComponent<TMP_Text>().text = towerPrefabs[2].GetComponent<Tower>().price + "";
        GameObject.Find("RailPrice").GetComponent<TMP_Text>().text = towerPrefabs[3].GetComponent<Tower>().price + "";


        p = GameObject.FindWithTag("Player").GetComponent<Player>();
        overlay = GameObject.FindWithTag("ui_overlay");
        overlay.SetActive(true);

        towerUI = GameObject.FindWithTag("ui_tower");
        towerUI.SetActive(false);

        upgradeUI = GameObject.FindWithTag("ui_upgrade");
        upgradeUI.SetActive(false);

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(currentTowerTile != null)
            {
                if (currentTowerTile.placedTower == null)
                {
                    EnableTowerUI();
                }
                else
                {
                    EnableUpgradeUI();
                }
            }
        }

        moneyText.text = p.money + "";
    }

    public void GetMoney(int money)
    {
        p.money += money;
    }

    public AudioClip GetDamageSound()
    {
        return enemyDamageSounds[Random.Range(0, enemyDamageSounds.Length)];
    }

    public void EnableTowerUI()
    {
        towerUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void EnableUpgradeUI()
    {
        upgradeUI.SetActive(true);
        Upgrade u1 = currentTowerTile.placedTower.upgradePath.GetUpgrade1();
        Upgrade u2 = currentTowerTile.placedTower.upgradePath.GetUpgrade2();

        // TODO should also disable button when null
        upgradeUI.transform.Find("U1").GetChild(0).GetComponent<TMP_Text>().text = u1 != null ? u1.text : "No More Upgrades";
        upgradeUI.transform.Find("U2").GetChild(0).GetComponent<TMP_Text>().text = u2 != null ? u2.text : "No More Upgrades";

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ApplyUpgradeOfCurrentTower(int which)
    {
        currentTowerTile.placedTower.upgradePath.ApplyUpgrade(which);
    }

    public
        void DisableUI()
    {
        towerUI.SetActive(false);
        upgradeUI.SetActive(false);
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
            DisableUI();
        }
    }

    public void RemoveEnemyFromAllTowers(Enemy e)
    {
        for(int i = 0; i < placedTowers.Count; i++)
        {
            placedTowers[i].enemiesInRange.Remove(e);
        }
    }

    bool roundFinishedSpawning = false;
    IEnumerator IterateRounds()
    {
        for (int i = 0; i < rounds.Length; i++)
        {
            roundText.text = "Round: " + (i + 1);
            Round round = rounds[i];
            roundFinishedSpawning = false;
            StartCoroutine(IterateWaves(round));
            while (!roundFinishedSpawning) yield return null;

            WaitForSeconds checkTime = new WaitForSeconds(1.0f);

            redoLoop:
            yield return checkTime;
            foreach(Enemy e in enemies)
            {
                if (e == null || e.dead) continue;
                goto redoLoop;
            }
            Debug.Log("FINISHED ROUND");
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
        roundFinishedSpawning = true;
        curWaveIdx = 0;
    }
    public void Lose()
    {
        defeatedMenuController.OnPlayerDefeated();
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
    SmallDrone = 0,
    Crawler,
    BigDrone,
    Stealth,
    DroneSpawnerBoss,
    WalkerBoss
}

[System.Serializable]
public enum TowerType
{
    Bomb,
    Generator,
    Turret,
    Rail
};