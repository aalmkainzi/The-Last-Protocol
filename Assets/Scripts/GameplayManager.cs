using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using PrimeTween;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices.WindowsRuntime;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;

    public List<Enemy> enemies;
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

    public EnemySpanwer[] spawners;
    int curRoundIdx = 0;

    public Vector3[] path1;
    public Vector3[] path2;

    public List<FixedTower> placedTowers;

    GameObject overlay;
    TMP_Text moneyText;
    TMP_Text roundText;

    public AudioClip[] booms;
    Player p;

    [SerializeField] private AudioClip[] enemyDamageSounds;

    GameObject towerUI;
    GameObject upgradeUI;
    public DefeatedMenuController defeatedMenuController;

    public RadarTower radioTower;
    public TowerPlacer towerPlacer;

    bool interactedWithTower = false;
    AudioSource audioSource;
    public AudioClip whyWouldThey;
    public AudioClip alertNearby;
    public AudioClip radioOld;
    public AudioClip thatSound;
    public AudioClip justRecieved;
    public Transform antenna;

    bool playedBeep = false;
    bool won = false;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Invoke(nameof(DoYouCopy), 2.0f);
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

        PrimeTweenConfig.SetTweensCapacity(1000);

        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        enemies = new();

        rounds = new Round[5]
        {
            new Round(new EnemyWave[] {new EnemyWave(EnemyType.SmallDrone, 3, 0.5f, 0, 1.0f), new EnemyWave(EnemyType.Crawler, 3, 0.5f, 0, 1.0f)}),
            new Round(new EnemyWave[] {new EnemyWave(EnemyType.SmallDrone, 2, 0.5f, 1, 0.0f), new EnemyWave(EnemyType.SmallDrone, 3, 0.5f, 0, 0.0f)}),
            new Round(new EnemyWave[] {new EnemyWave(EnemyType.BigDrone, 2, 0.6f, 0, 0.0f), new EnemyWave(EnemyType.BigDrone, 2, 0.6f, 1, 0.0f), new EnemyWave(EnemyType.Crawler, 2, 0.6f, 0, 0.0f), new EnemyWave(EnemyType.Crawler, 2, 0.6f, 1, 0.0f)}),
            new Round(new EnemyWave[] {new EnemyWave(EnemyType.DroneSpawnerBoss, 1, 0.6f, 0, 0.0f), new EnemyWave(EnemyType.Crawler, 2, 0.6f, 0, 0.0f), new EnemyWave(EnemyType.Crawler, 2, 0.6f, 1, 0.0f)}),
            new Round(new EnemyWave[] {new EnemyWave(EnemyType.Stealth, 2, 0.6f, 0, 3.0f)}),
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

        GameObject.Find("BombPrice").GetComponent<Text>().text = towerPrefabs[0].GetComponent<Tower>().price + "";
        GameObject.Find("GeneratorPrice").GetComponent<Text>().text = towerPrefabs[1].GetComponent<Tower>().price + "";
        GameObject.Find("TurretPrice").GetComponent<Text>().text = towerPrefabs[2].GetComponent<Tower>().price + "";
        GameObject.Find("RailPrice").GetComponent<Text>().text = towerPrefabs[3].GetComponent<Tower>().price + "";


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

        placedTowers = new List<FixedTower>();
        Enemy.curId = 0;

        StartCoroutine(SpinAntenna());

    }

    IEnumerator SpinAntenna()
    {
        while (true)
        {
            if(roundFinished)
            {
                antenna.Rotate(new Vector3(0, 40 * Time.deltaTime, 0));

            }
            yield return null;
        }
    }

    void DoYouCopy()
    {
        audioSource.Play();
        Invoke(nameof(WhyWouldThey), 9.0f);
    }

    void WhyWouldThey()
    {
        audioSource.PlayOneShot(whyWouldThey);
    }

    void Update()
    {
        if (won)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(!towerUI.activeSelf && !upgradeUI.activeSelf)
            {
               EnableTowerUI();
            }
            else
            {
                DisableUI();
            }
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            if(player.nearRadio && roundFinished)
            {
                if(!interactedWithTower)
                {
                    interactedWithTower = true;
                    audioSource.PlayOneShot(radioOld);
                    Invoke(nameof(AlertNearbyAudio), 6.5f);
                }
                if(curRoundIdx == rounds.Length)
                {
                    PlayWinAudio();
                }
                else
                {
                    roundFinished = false;
                    StartCoroutine(IterateWaves(rounds[curRoundIdx++]));
                }
            }
            else if(player.nearTower != null)
            {
                Debug.Log("UPGRADE MENU");
                EnableUpgradeUI();
            }
        }

        moneyText.text = p.money + "";
    }

    void PlayWinAudio()
    {
        won = true;
        audioSource.PlayOneShot(justRecieved);
        Invoke(nameof(GoToLevel2), 10f);
    }

    void GoToLevel2()
    {
        SceneManager.LoadScene("level2");
    }

    void AlertNearbyAudio()
    {
        audioSource.PlayOneShot(alertNearby);
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

        // TODO should also disable button when null
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

    public void RemoveEnemyFromAllTowers(Enemy e)
    {
        for(int i = 0; i < placedTowers.Count; i++)
        {
            placedTowers[i].enemiesInRange.Remove(e);
        }
    }

    bool roundFinishedSpawning = false;

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

        WaitForSeconds checkRoundClearWaitTime = new WaitForSeconds(1.5f);
    redoLoop:
        yield return checkRoundClearWaitTime;
        foreach (Enemy e in enemies)
        {
            if (e == null || e.dead) continue;
            goto redoLoop;
        }

        roundFinished = true;
        
        radioTower.PlayBeeps();
        if(!playedBeep)
        {
            Invoke(nameof(ThatSound), 1.5f);
            playedBeep = true;
        }
    }

    void ThatSound()
    {
        audioSource.PlayOneShot(thatSound);
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