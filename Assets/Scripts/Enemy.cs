using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public int health;
    public int gearsWhenKilled;
    public float speed;
    public int power;
    Animator animator;
    RadarTower radarTower;
    NavMeshAgent agent;
    public bool flying;
    // Rigidbody rb;
    void Start()
    {
        // rb = GetComponent<Rigidbody>();
        radarTower = GameObject.FindWithTag("radioTower").GetComponent<RadarTower>();
        agent = GetComponent<NavMeshAgent>();

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

        agent.SetDestination(radarTower.gameObject.transform.position);

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
