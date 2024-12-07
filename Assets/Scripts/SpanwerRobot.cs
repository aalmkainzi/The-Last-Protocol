using System.Collections;
using UnityEngine;
using PrimeTween;

public class SpanwerRobot : Enemy
{
    GameObject dronePrefab;
    public GameObject spawner1;
    public GameObject spawner2;
    bool blowUpOnRadio;

    public GameObject particle;
    protected override void Start()
    {
        base.Start();
        dronePrefab = gameplayManager.enemyPrefabs[0];
        StartCoroutine(SpawnDrones());
        particle.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
    }

    IEnumerator SpawnDrones()
    {
        GameObject[] spawners = { spawner1, spawner2 };
        int curSpawner = 0;
        WaitForSeconds spawnDelay = new WaitForSeconds(1.5f);
        while(true)
        {
            Vector3 spawnPos = spawners[curSpawner].transform.position;
            spawnPos.y -= 1.5f;
            GameObject newObj = Instantiate(dronePrefab, spawnPos, Quaternion.identity);
            Vector3 newPos = spawners[curSpawner].transform.position;
            newObj.transform.GetChild(0).position = newPos;

            Enemy newE = newObj.transform.GetChild(0).GetComponent<Enemy>();

            Vector3[] ogPath;
            if (pathId == 0)
                ogPath = gameplayManager.path1;
            else
                ogPath = gameplayManager.path2;

            Vector3[] randomPath = new Vector3[ogPath.Length - curTargetIdx];

            for (int j = 0; j < randomPath.Length - 1; j++)
            {
                randomPath[j] = ogPath[curTargetIdx + j] + new Vector3(Random.Range(-2.5f, 2.5f), 0, Random.Range(-2.0f, 2.0f));
            }

            randomPath[randomPath.Length - 1] = ogPath[ogPath.Length - 1];

            newE.path = randomPath;
            newE.navCor = newE.StartCoroutine(newE.MoveAlongPath());

            curSpawner = curSpawner ^ 1;
            yield return spawnDelay;
        }
    }

    public override void Die()
    {
        base.Die();
        GetComponent<AudioSource>().clip = gameplayManager.booms[Random.Range(0, gameplayManager.booms.Length)];
        GetComponent<AudioSource>().Play();
        StartCoroutine(SpinInSpiral());
        particle.SetActive(true);
    }

    protected override void Attack()
    {
        // Debug.Log("BOSS ATTACKING");
        agent.enabled = false;
        blowUpOnRadio = true;
        PrimeTween.Tween.Position(transform, radarTower.transform.position, duration: 1.0f);
    }

    private void OnCollisionEnter(Collision other)
    {
        // Debug.Log("Colliding with " + other.gameObject.name);
        if (blowUpOnRadio && other.gameObject.CompareTag("radioTower"))
        {
            radarTower.TakeDamage(power);
            Die();
        }
    }
}
