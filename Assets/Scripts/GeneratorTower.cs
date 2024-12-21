using UnityEngine;
using UnityEngine.Rendering;

public class GeneratorTower : FixedTower
{
    GameplayManager gameplayManager;
    public int moneyPerAttack = 10;
    float lastTimeMoneyMade;

    protected override void Start()
    {
        base.Start();
        gameplayManager = GameObject.FindWithTag("gameplayManager").GetComponent<GameplayManager>();
    }

    protected override void Update()
    {
        if (!reloading && ammo == 0)
        {
            reloading = true;
            Reload();
        }

        bool shouldFire = !reloading && ammo > 0 && Time.time - lastTimeMoneyMade >= timeBetweenShots;
        if (shouldFire)
        {
            audioSource.PlayOneShot(attackSound);
            Fire(null);
            lastTimeMoneyMade = Time.time;
        }
    }

    protected override void Fire(Enemy target)
    {
        gameplayManager.GetMoney(moneyPerAttack);
    }
}
