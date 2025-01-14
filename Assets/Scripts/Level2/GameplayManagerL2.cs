using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using PrimeTween;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices.WindowsRuntime;

public class GameplayManagerL2 : MonoBehaviour
{
    public static GameplayManagerL2 instance;

    // public List<Enemy> enemies;
    public Player player;
    public GameObject[] enemyPrefabs;
    public GameObject[] towerPrefabs;

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
    bool roundFinished = true;

    public EnemySpawnerL2[] spawners;
    int curRoundIdx = 0;

    public List<FixedTowerL2> placedTowers;

    GameObject overlay;
    TMP_Text moneyText;
    TMP_Text roundText;

    public AudioClip[] booms;
    Player p;

    [SerializeField] private AudioClip[] enemyDamageSounds;

    GameObject towerUI;
    GameObject upgradeUI;
    public DefeatedMenuController defeatedMenuController;

    public Wagon wagon;
    public TowerPlacer towerPlacer;

    AudioSource audioSource;

    bool playedBeep = false;
    bool won = false;

    public int unlockedSpawners = 0;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        roundFinished = true;
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        // enemies = new();

        /*        rounds = new Round[5]
                {
                    new Round(new EnemyWave[] {new EnemyWave(EnemyType.SmallDrone, 3, 0.5f, 0, 1.0f), new EnemyWave(EnemyType.Crawler, 3, 0.5f, 0, 1.0f)}),
                    new Round(new EnemyWave[] {new EnemyWave(EnemyType.SmallDrone, 2, 0.5f, 1, 0.0f), new EnemyWave(EnemyType.SmallDrone, 3, 0.5f, 0, 0.0f)}),
                    new Round(new EnemyWave[] {new EnemyWave(EnemyType.BigDrone, 2, 0.6f, 0, 0.0f), new EnemyWave(EnemyType.BigDrone, 2, 0.6f, 1, 0.0f), new EnemyWave(EnemyType.Crawler, 2, 0.6f, 0, 0.0f), new EnemyWave(EnemyType.Crawler, 2, 0.6f, 1, 0.0f)}),
                    new Round(new EnemyWave[] {new EnemyWave(EnemyType.DroneSpawnerBoss, 1, 0.6f, 0, 0.0f), new EnemyWave(EnemyType.Crawler, 2, 0.6f, 0, 0.0f), new EnemyWave(EnemyType.Crawler, 2, 0.6f, 1, 0.0f)}),
                    new Round(new EnemyWave[] {new EnemyWave(EnemyType.Stealth, 2, 0.6f, 0, 3.0f)}),
                };*/

        rounds = new Round[3]
        {
            new Round(new EnemyWave[]{ new EnemyWave(EnemyType.SmallDrone, 3, 5f, 0, 2.0f) }),
            new Round(new EnemyWave[]{ new EnemyWave(EnemyType.SmallDrone, 3, 5f, 0, 1.0f) }),
            new Round(new EnemyWave[]{ new EnemyWave(EnemyType.Grunt, 1, 5f, 0, 1.0f) }),
        };

/*        GameObject.Find("BombPrice").GetComponent<Text>().text = towerPrefabs[0].GetComponent<Tower>().price + "";
        GameObject.Find("GeneratorPrice").GetComponent<Text>().text = towerPrefabs[1].GetComponent<Tower>().price + "";
        GameObject.Find("TurretPrice").GetComponent<Text>().text = towerPrefabs[2].GetComponent<Tower>().price + "";
        GameObject.Find("RailPrice").GetComponent<Text>().text = towerPrefabs[3].GetComponent<Tower>().price + "";
*/
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

        placedTowers = new List<FixedTowerL2>();
        EnemyL2.curId = 0;

        StartCoroutine(IterateRounds());
    }

    void Update()
    {
        if (won)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!towerUI.activeSelf && !upgradeUI.activeSelf)
            {
                EnableTowerUI();
            }
            else
            {
                DisableUI();
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (player.nearTower != null)
            {
                Debug.Log("UPGRADE MENU");
                EnableUpgradeUI();
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
        Upgrade u1 = player.nearTower.upgradePath.GetUpgrade1();
        Upgrade u2 = player.nearTower.upgradePath.GetUpgrade2();

        // TODO should also disable button when no more upgrades
        upgradeUI.transform.Find("U1").GetChild(0).GetComponent<TMP_Text>().text = u1 != null ? u1.text : "No More Upgrades";
        upgradeUI.transform.Find("U2").GetChild(0).GetComponent<TMP_Text>().text = u2 != null ? u2.text : "No More Upgrades";

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ApplyUpgradeOfCurrentTower(int which)
    {
        player.nearTower.upgradePath.ApplyUpgrade(which);
    }

    public void DisableUI()
    {
        towerUI.SetActive(false);
        upgradeUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SelectTower(int towerTy)
    {
        Debug.Log("SELECT TOWER CALLED WITH " + towerTy);
        Tower towerPrefab = towerPrefabs[(int)towerTy].GetComponent<Tower>();
        if (p.money >= towerPrefab.price)
        {
            p.money -= towerPrefab.price;
            towerPlacer.SetTowerPrefab(towerPrefab.gameObject);
            DisableUI();
        }
    }

    public void RemoveEnemyFromAllTowers(EnemyL2 e)
    {
        for (int i = 0; i < placedTowers.Count; i++)
        {
            placedTowers[i].enemiesInRange.Remove(e);
        }
    }

    bool roundFinishedSpawning = false;

    // idea, each spawner has a queue of enemies, only starts when some flag is set
    IEnumerator IterateRounds()
    {
        WaitForSeconds afterRoundDelay = new WaitForSeconds(5);
        foreach(Round r in rounds)
        {
            Debug.Log("NEW ROUND");
            StartCoroutine(IterateWaves(r));
            yield return afterRoundDelay;
        }
    }

    IEnumerator IterateWaves(Round round)
    {
        Debug.Log("ITERATING WAVES");
        yield return new WaitForSeconds(1.5f);
        int curWaveIdx = 0;
        for (int i = 0; i < round.waves.Length; i++)
        {
            EnemyWave curWave = round.waves[curWaveIdx];
            spawners[curWave.spawnerIdx].SpawnWave(curWave);
            curWaveIdx++;
            yield return new WaitForSeconds(curWave.afterWaveDelay);
        }
        roundFinishedSpawning = true;
        curWaveIdx = 0;

        roundFinished = true;
    }
    public void Lose()
    {
        defeatedMenuController.OnPlayerDefeated();
    }

    public AudioClip GetRandomBoom()
    {
        return booms[Random.Range(0, booms.Length)];
    }

    [System.Serializable]
    public struct EnemyWave
    {
        public GameplayManagerL2.EnemyType type;
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
        Spider,
        Grunt,
        Stealth,
        Mech
    }

    [System.Serializable]
    public enum TowerType
    {
        Bomb,
        Generator,
        Turret,
        Rail
    };
}

