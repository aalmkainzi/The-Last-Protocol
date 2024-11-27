using System.Collections;
using UnityEngine;

public class SmallDrone : Enemy
{
    GameObject particle1;
    protected override void Start()
    {
        base.Start();
        particle1 = transform.Find("Particle").gameObject;

        particle1.SetActive(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void Die()
    {
        base.Die();
        particle1.SetActive(true);
    }

    protected override void Attack()
    {
        agent.enabled = true;
        agent.SetDestination(radarTower.transform.position);
        StartCoroutine(BlowUpOnImpact());
    }

    IEnumerator BlowUpOnImpact()
    {
        while(true)
        {
            if(Vector3.Distance(transform.position, radarTower.transform.position) <= 0.75f)
            {
                radarTower.TakeDamage(5);
                Die();
            }
        }
    }
}
