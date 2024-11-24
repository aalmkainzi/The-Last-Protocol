using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public int gearsWhenKilled;
    public float speed;
    public int power;
    Animator animator;
    RadarTower radarTower;

    void Start()
    {
        animator = GetComponent<Animator>();
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
