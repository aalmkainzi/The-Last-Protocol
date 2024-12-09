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
    public float reloadTime;

    UpgradePath upgradePath;

    public Renderer rend;
    protected virtual void Start()
    {
    }

    public void Reload()
    {
        ammo = 0;
        Invoke("FillAmmo", reloadTime);
    }

    void FillAmmo()
    {
        ammo = fullAmmo;
        reloading = false;
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
