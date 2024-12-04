using DG.Tweening;
using UnityEngine;

public class EnemyCrawler : Enemy
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Die()
    {
        base.Die();
        agent.isStopped = true;
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
