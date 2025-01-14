using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using PrimeTween;
using UnityEngine.UIElements;
using UnityEngine.VFX;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using System;

public class EnemyL2 :MonoBehaviour, IEquatable<Enemy>
{
    public static int curId = 0;

    public int id;
    public int health;
    public int gearsWhenKilled;
    public float speed;
    public int power;
    public Animator animator;
    protected Wagon wagon;
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

    bool playerInTrigger;

    public int pathId;


    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        id = curId++;
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        wagon = GameObject.FindWithTag("Wagon").GetComponent<Wagon>();
        agent.speed = speed;
        if (flying)
        {
            float seed = UnityEngine.Random.Range(1.25f, 3.5f);
            flyingTween = Tween.PositionY(transform, startValue: transform.position.y, endValue: transform.position.y + seed, duration: UnityEngine.Random.Range(2.0f, 4.0f), ease: Ease.InOutSine, cycles: -1, cycleMode: CycleMode.Yoyo);
        }

        InvokeRepeating(nameof(FollowWagon), 0, 2.0f);
    }
    protected virtual void Update()
    {
        Vector3 posIgnorY = transform.position;
        posIgnorY.y = wagon.transform.position.y;
        float distanceFromGoal = Vector3.Distance(wagon.transform.position, posIgnorY);
        if (distanceFromGoal <= attackRange && Time.time - lastAttackTime >= timeBetweenAttacks)
        {
            lastAttackTime = Time.time;
            Attack();
        }
    }
    
    void FollowWagon()
    {
        agent.SetDestination(wagon.transform.position);
    }

    protected virtual void Attack()
    {
        Debug.Log("actually we here for some reason");
        animator.SetTrigger("attack");
        wagon.TakeDamage(power);
    }

    public bool TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            Die();
            return true;
        }
        else
        {
            audioSource.PlayOneShot(GameplayManagerL2.instance.GetDamageSound());
        }
        return false;
    }

    public virtual void Die()
    {
        StopAllCoroutines();
        dead = true;
        GameplayManagerL2.instance.RemoveEnemyFromAllTowers(this);
        GameplayManagerL2.instance.GetMoney(gearsWhenKilled);

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
