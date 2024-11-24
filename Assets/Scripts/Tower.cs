using UnityEngine;

public class Tower : MonoBehaviour
{
    public int price;
    public int sellPrice;
    public int health;
    public int power;
    public int fullHealth;
    public int fullAmmo;
    public int ammo;
    public float timeBetweenShots;
    public bool reloading;
    public bool canDamageMetal;
    public bool canDetectCamo;
    public bool canAttackFlying;
    
    Animator animator;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Reload()
    {
        ammo = 0;
        animator.SetTrigger("reload"); // this should set the ammo on OnStateExit
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!reloading && ammo == 0)
        {
            reloading = true;
            Reload();
        }
    }
}
