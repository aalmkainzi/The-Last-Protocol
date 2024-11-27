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
    public Animator animator;
    protected RadarTower radarTower;
    public bool flying;
    
    public Vector3[] path;
    int curTargetIdx;

    public Coroutine navCor;
    private Coroutine attackCor;

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

    protected virtual void Start()
    {
        id = curId++;
        gameplayManager = GameObject.FindWithTag("gameplayManager").GetComponent<GameplayManager>();
        player = GameObject.FindWithTag("player").GetComponent<Player>();
        radarTower = GameObject.FindWithTag("radioTower").GetComponent<RadarTower>();
        agent.speed = speed;
        if (flying)
        {
            float seed = UnityEngine.Random.Range(1.25f, 3.5f);
            flyingTween = transform.DOMoveY(transform.position.y + seed, UnityEngine.Random.Range(2.0f, 4.0f), false).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
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
        float distanceFromGoal = Vector3.Distance(radarTower.gameObject.transform.position, transform.position);
        if (distanceFromGoal <= 1.5f)
        {
            Attack();
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

    protected virtual void Attack()
    {
        animator.SetTrigger("attack");
        radarTower.TakeDamage(power);
    }

    public bool TakeDamage(int dmg)
    {
        health -= dmg;
        if(health <= 0)
        {
            gameplayManager.RemoveEnemyFromAllTowers(this);
            Debug.Log("DIED");
            Die();
            return true;
        }
        return false;
    }

    public virtual void Die()
    {
        if(attackCor != null)
            StopCoroutine(attackCor);        
        StopCoroutine(navCor);

        agent.enabled = false;

        StartCoroutine(SpinInSpiral());
        
        if (flyingTween != null) flyingTween.Kill();
    }

    IEnumerator SpinInSpiral()
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
        Destroy(flying ? transform.parent.gameObject : gameObject);
    }

    public bool Equals(Enemy other)
    {
        return id == other.id;
    }
}
