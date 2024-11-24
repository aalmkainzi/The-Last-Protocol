using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedTower : Tower
{
    List<Enemy> enemiesInRange;
    public SphereCollider rangeCollider;

    protected override void Start()
    {
        base.Start();
        rangeCollider = GetComponent<SphereCollider>();
        enemiesInRange = new List<Enemy>();
    }

    protected override void Update()
    {
        base.Update();
    }

    public IEnumerator Attack()
    {
        while (true)
        {
            if (enemiesInRange.Count > 0 && !reloading && ammo > 0)
            {
                Enemy e = enemiesInRange[0];
                Vector3 ePos = e.transform.position;
                ePos.y = transform.position.y;
                transform.LookAt(ePos);
                ammo -= 1;
                yield return timeBetweenShots;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        enemiesInRange.Add(other.gameObject.GetComponent<Enemy>());
    }

    private void OnTriggerExit(Collider other)
    {
        enemiesInRange.Remove(other.gameObject.GetComponent<Enemy>());
    }
}
