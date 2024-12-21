using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedTower : Tower
{
    public List<Enemy> enemiesInRange;
    // public SphereCollider rangeCollider;
    float lastShot;

    protected override void Start()
    {
        base.Start();
        // rangeCollider = GetComponent<SphereCollider>();
        enemiesInRange = new List<Enemy>();
        // StartCoroutine(Attack());
    }

    protected override void Update()
    {
        base.Update();

        //Debug.Log("enemies in range" + enemiesInRange.Count);
        //Debug.Log("is reloading: " + reloading);
        //Debug.Log("cur ammo: " + ammo);

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
                Enemy e = enemiesInRange[0];
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

    protected virtual void Fire(Enemy target)
    {
        target.TakeDamage(power);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("ENEMY ENTERED TRIGGER");
        Enemy e = other.gameObject.GetComponent<Enemy>();
        if (!e.dead)
        {
            enemiesInRange.Add(other.gameObject.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        enemiesInRange.Remove(other.gameObject.GetComponent<Enemy>());
    }
}
