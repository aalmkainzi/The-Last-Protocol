using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedTowerL2 : Tower
{
    public List<EnemyL2> enemiesInRange;
    // public SphereCollider rangeCollider;
    float lastShot;

    protected override void Start()
    {
        base.Start();
        enemiesInRange = new List<EnemyL2>();
    }

    protected override void Update()
    {
        base.Update();
        
        bool shouldFire = !reloading && ammo > 0 && Time.time - lastShot >= timeBetweenShots;

        if (enemiesInRange.Count > 0)
        {
            if (enemiesInRange[0] == null)
            {
                enemiesInRange.RemoveAt(0);
                return;
            }
            Vector3 lookAt = enemiesInRange[0].transform.position;
            lookAt.y = transform.position.y;
            transform.LookAt(lookAt);
            if (shouldFire)
            {
                EnemyL2 e = enemiesInRange[0];
                Vector3 ePos = e.transform.position;
                ePos.y = transform.position.y;
                transform.LookAt(ePos);
                ammo -= 1;
                audioSource.PlayOneShot(attackSound);
                Fire(e);
                lastShot = Time.time;
            }
        }
    }

    protected virtual void Fire(EnemyL2 target)
    {
        target.TakeDamage(power);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("ENEMY ENTERED TRIGGER");
        EnemyL2 e = other.gameObject.GetComponent<EnemyL2>();
        if (!e.dead)
        {
            enemiesInRange.Add(e);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        enemiesInRange.Remove(other.gameObject.GetComponent<EnemyL2>());
    }
}
