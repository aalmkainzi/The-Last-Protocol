using System.Collections;
using UnityEngine;

public class EnemySpawnerL2 : MonoBehaviour
{
    public float offsetX;
    public float offsetZ;

    IEnumerator SpawnWaveLoop(GameplayManagerL2.EnemyWave wave)
    {
        GameObject prefab = GameplayManagerL2.instance.enemyPrefabs[(int)wave.type];

        WaitForSeconds delay = new WaitForSeconds(wave.timeBetweenEach);

        for (int i = 0; i < wave.nb; i++)
        {
            Vector3 spawnOffset = transform.position + new Vector3(Random.Range(-offsetX, offsetX), 0, Random.Range(-offsetZ, offsetZ));

            GameObject newObj = Instantiate(prefab, spawnOffset, Quaternion.identity);
            EnemyL2 newE = newObj.GetComponent<EnemyL2>();

            yield return delay;
        }
    }

    public void SpawnWave(GameplayManagerL2.EnemyWave wave)
    {
        StartCoroutine(SpawnWaveLoop(wave));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + new Vector3(offsetX, 0, offsetZ), 0.5f);
        Gizmos.DrawSphere(transform.position + new Vector3(-offsetX, 0, -offsetZ), 0.5f);
    }
}
