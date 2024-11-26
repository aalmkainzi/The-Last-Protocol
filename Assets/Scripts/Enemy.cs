using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.UIElements;
using UnityEngine.VFX;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    public int health;
    public int gearsWhenKilled;
    public float speed;
    public int power;
    Animator animator;
    RadarTower radarTower;
    public bool flying;
    public Vector3[] path;
    public NavMeshAgent agent;

    public enum AttackPlayerBehaviour {
        None, Chase, StopAndShoot, MoveAndShoot
    };

    bool playerInTrigger;

    public AttackPlayerBehaviour attackPlayer;
    
    GameplayManager gameplayManager;

    void Start()
    {
        radarTower = GameObject.FindWithTag("radioTower").GetComponent<RadarTower>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        if (flying)
        {
            Transform flyer = transform.GetChild(0);
            animator = flyer.gameObject.GetComponent<Animator>();
            float seed = Random.Range(1.25f, 3.5f);
            flyer.DOMoveY(flyer.position.y + seed, Random.Range(2.0f, 4.0f), false).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
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
        int cur = 0;
        agent.SetDestination(path[0]);
        yield return null;
        while (cur < path.Length - 1)
        {
            Vector3 pos = transform.position;
            pos.y = path[cur].y;
            if (Vector3.Distance(path[cur], pos) <= 2.0f)
            {
                cur++;
                Debug.Log("cur: " + cur);
                agent.SetDestination(path[cur]);
            }
            yield return null;
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
