using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using PrimeTween;
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
    public Animator animator;
    protected RadarTower radarTower;
    public bool flying;
    
    public Vector3[] path;
    protected int curTargetIdx;

    public Coroutine navCor;
    private Coroutine attackCor;

    public NavMeshAgent agent;
    public Player player;
    public float sightRange;
    public float attackRange;
    Tween flyingTween;
    public AudioSource audioSource;
    public float timeBetweenAttacks;
    public float lastAttackTime;
    public bool dead;
    public enum AttackPlayerBehaviour {
        None, Chase, StopAndShoot, MoveAndShoot
    };

    bool playerInTrigger;

    public int pathId;

    public AttackPlayerBehaviour attackPlayer;
    
    protected virtual void Start()
    {
        // audioSource = GetComponent<AudioSource>();
        id = curId++;
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        radarTower = GameObject.FindWithTag("radioTower").GetComponent<RadarTower>();
        agent.speed = speed;
        if (flying)
        {
            float seed = UnityEngine.Random.Range(1.25f, 3.5f);
            flyingTween = PrimeTween.Tween.PositionY(transform, startValue: transform.position.y, endValue: transform.position.y + seed, duration: UnityEngine.Random.Range(2.0f, 4.0f), ease: Ease.InOutSine, cycles: -1, cycleMode: CycleMode.Yoyo);
        }

        if (attackPlayer == AttackPlayerBehaviour.Chase)
        {
            attackCor = StartCoroutine(ChasePlayerIfInRange());
        }
        else if (attackPlayer == AttackPlayerBehaviour.StopAndShoot)
        {
            attackCor = StartCoroutine(StopAndShootPlayerIfInRange());
        }
        else if (attackPlayer == AttackPlayerBehaviour.MoveAndShoot)
        {
            attackCor = StartCoroutine(ShootPlayerIfInRange());
        }
    }
    protected virtual void Update()
    {
        Vector3 posIgnorY = transform.position;
        posIgnorY.y = radarTower.transform.position.y;
        float distanceFromGoal = Vector3.Distance(radarTower.gameObject.transform.position, posIgnorY);
        if (distanceFromGoal <= attackRange && Time.time - lastAttackTime >= timeBetweenAttacks)
        {
            lastAttackTime = Time.time;
            Attack();
        }
    }
    public IEnumerator MoveAlongPath()
    {
        agent.SetDestination(path[curTargetIdx]);
        yield return null;
        while (curTargetIdx < path.Length - 1)
        {
            // if(agent.active)
            Vector3 pos = transform.position;
            pos.y = path[curTargetIdx].y;
            if (Vector3.Distance(path[curTargetIdx], pos) <= 2.5f)
            {
                curTargetIdx++;
                Vector3 randOffset = new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f));
                agent.SetDestination(path[curTargetIdx] + randOffset);
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

    protected virtual void Attack()
    {
        Debug.Log("actually we here for some reason");
        animator.SetTrigger("attack");
        radarTower.TakeDamage(power);
    }

    int ii = 0;
    public bool TakeDamage(int dmg)
    {
        health -= dmg;
        if(health <= 0)
        {
            Die();
            return true;
        }
        else
        {
            //Debug.Log("PLAYING SOUND " + ii++);
            audioSource.PlayOneShot(GameplayManager.instance.GetDamageSound());
        }
        return false;
    }

    public virtual void Die()
    {
        /*        if(attackCor != null)
                    StopCoroutine(attackCor);        
                StopCoroutine(navCor);*/
        if (dead) return;
        StopAllCoroutines();
        dead = true;
        GameplayManager.instance.RemoveEnemyFromAllTowers(this);
        GameplayManager.instance.GetMoney(gearsWhenKilled);

        // agent.isStopped = true;

        // transform.DOKill();
        // if (flyingTween != null) flyingTween.Kill();
    }

    protected IEnumerator SpinInSpiral()
    {
        float timeElapsed = 0f;
        float rotationSpeed = 200f;
        float spiralFrequency = 3f;
        float spiralSpeed = 1.0f;
        float startTime = Time.time;
        float downwardSpeed = 4.0f;
        Vector3 initialPosition = transform.position;
        while (Time.time - startTime < 5.0f)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            // Calculate the new position in the spiral relative to the initial position
            float x = Mathf.Cos(timeElapsed * spiralFrequency) * timeElapsed * spiralSpeed;
            float z = Mathf.Sin(timeElapsed * spiralFrequency) * timeElapsed * spiralSpeed;

            // Update the position while keeping the current Y-axis value
            transform.position = initialPosition + new Vector3(x, 0, z);
            initialPosition -= new Vector3(0, downwardSpeed * Time.deltaTime, 0);

            // Increment time
            timeElapsed += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }
        // transform.DOKill();
        Destroy(transform.parent.gameObject);
    }

    public bool Equals(Enemy other)
    {
        return id == other.id;
    }
}
