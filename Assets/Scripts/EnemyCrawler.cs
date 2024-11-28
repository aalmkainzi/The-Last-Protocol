using DG.Tweening;
using UnityEngine;

public class EnemyCrawler : Enemy
{
    GameObject particle1;
    protected override void Start()
    {
        base.Start();
        particle1 = transform.Find("Particle").gameObject;

        particle1.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Die()
    {
        base.Die();
        particle1.SetActive(true);
        animator.SetBool("die", true);
        Invoke(nameof(DestroyCrawler), 1.75f);
    }

    void DestroyCrawler()
    {
        Destroy(transform.parent.gameObject);
    }

    protected override void Attack()
    {
        agent.isStopped = true;
        animator.SetBool("attack", true);
        radarTower.TakeDamage(power);
    }
}
