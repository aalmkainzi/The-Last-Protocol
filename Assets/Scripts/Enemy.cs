using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.UIElements;
using UnityEngine.VFX;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using System;

public class Enemy : MonoBehaviour, IEquatable<Enemy>
{
    public static int curId = 0;

    public int id;
    public int health;
    public int gearsWhenKilled;
    public float speed;
    public int power;
    Animator animator;
    RadarTower radarTower;
    public bool flying;
    
    public Vector3[] path;
    int curTargetIdx;
    public Coroutine navCor;

    public NavMeshAgent agent;
    public Player player;
    public float sightRange;

    Tween flyingTween;
    public enum AttackPlayerBehaviour {
        None, Chase, StopAndShoot, MoveAndShoot
    };

    bool playerInTrigger;

    public AttackPlayerBehaviour attackPlayer;
    
    GameplayManager gameplayManager;

    void Start()
    {
        id = curId++;
        player = GameObject.FindWithTag("player").GetComponent<Player>();
        radarTower = GameObject.FindWithTag("radioTower").GetComponent<RadarTower>();
        // agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        if (flying)
        {
            //Transform flyer = transform.GetChild(0);
            animator = gameObject.GetComponent<Animator>();
            float seed = UnityEngine.Random.Range(1.25f, 3.5f);
            flyingTween = transform.DOMoveY(transform.position.y + seed, UnityEngine.Random.Range(2.0f, 4.0f), false).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            animator = GetComponent<Animator>();
        }

        if (attackPlayer == AttackPlayerBehaviour.Chase)
        {
            StartCoroutine(ChasePlayerIfInRange());
        }
        else if (attackPlayer == AttackPlayerBehaviour.StopAndShoot)
        {
            StartCoroutine(StopAndShootPlayerIfInRange());
        }
        else if (attackPlayer == AttackPlayerBehaviour.MoveAndShoot)
        {
            StartCoroutine(ShootPlayerIfInRange());
        }
    }

    public IEnumerator MoveAlongPath()
    {
        agent.SetDestination(path[curTargetIdx]);
        yield return null;
        while (curTargetIdx < path.Length - 1)
        {
            Vector3 pos = transform.position;
            pos.y = path[curTargetIdx].y;
            if (Vector3.Distance(path[curTargetIdx], pos) <= 2.0f)
            {
                curTargetIdx++;
                agent.SetDestination(path[curTargetIdx]);
            }
            yield return null;
        }
    }

    IEnumerator ChasePlayerIfInRange()
    {
        while(true)
        {
            Vector3 pos = transform.position;
            pos.y = player.gameObject.transform.position.y;
            if(Vector3.Distance(pos, player.transform.position) <= sightRange)
            {
                StopCoroutine(navCor);
                while(Vector3.Distance(pos, player.transform.position) <= sightRange)
                {
                    agent.SetDestination(player.transform.position);
                    pos = transform.position;
                    pos.y = player.gameObject.transform.position.y;
                }
                navCor = StartCoroutine(MoveAlongPath());
                yield return null;
            }
        }
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

    public bool TakeDamage(int dmg)
    {
        health -= dmg;
        if(health <= 0)
        {
            Debug.Log("DIED");
            Die();
            return true;
        }
        return false;
    }

    public void Die()
    {
        StopAllCoroutines();
        if (flyingTween != null) flyingTween.Kill();

        transform.position = new Vector3(999, 999, 999);
        transform.localScale = Vector3.zero;
        this.enabled = false;
        agent.isStopped = true;
        agent.enabled = false;
    }

    public bool Equals(Enemy other)
    {
        return id == other.id;
    }
}
