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
    }

    protected override void Attack()
    {
        agent.enabled = false;
        animator.SetBool("attack", true);
        Invoke(nameof(Die), 1.0f);
    }
}
