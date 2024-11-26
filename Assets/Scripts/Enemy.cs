using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class Enemy : MonoBehaviour
{
    public int health;
    public int gearsWhenKilled;
    public float speed;
    public int power;
    Animator animator;
    RadarTower radarTower;
    public bool flying;
    public Tween pathTween;
    public enum AttackPlayerBehaviour {
        None, Chase, StopAndShoot, MoveAndShoot
    };

    bool playerInTrigger;

    public AttackPlayerBehaviour attackPlayer;
    
    GameplayManager gameplayManager;

    void Start()
    {
        radarTower = GameObject.FindWithTag("radioTower").GetComponent<RadarTower>();

        if (flying)
        {
            animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
            float seed = Random.Range(1.25f, 3.5f);
            Transform flyer = transform.GetChild(0);
            flyer.DOMoveY(flyer.position.y + seed, Random.Range(2.0f, 4.0f), false).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            animator = GetComponent<Animator>();
        }

        if(attackPlayer == AttackPlayerBehaviour.Chase)
        {
            StartCoroutine(ChasePlayerIfInRange());
        }
        else if(attackPlayer == AttackPlayerBehaviour.StopAndShoot)
        {
            StartCoroutine(StopAndShootPlayerIfInRange());
        }
        else if(attackPlayer == AttackPlayerBehaviour.MoveAndShoot)
        {
            StartCoroutine(ShootPlayerIfInRange());
        }
    }

    IEnumerator ChasePlayerIfInRange()
    {
        yield return null;
    }

    IEnumerator StopAndShootPlayerIfInRange()
    {
        yield return null;
    }

    IEnumerator ShootPlayerIfInRange()
    {
        yield return null;
    }

    void Update()
    {
        float distanceFromGoal = Vector3.Distance(radarTower.gameObject.transform.position, transform.position);
        if(distanceFromGoal <= 1.5f)
        {
            Attack();
        }
    }

    void Attack()
    {
        animator.SetTrigger("attack");
        radarTower.TakeDamage(power);
    }
}
