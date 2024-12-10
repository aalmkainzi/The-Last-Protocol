using UnityEngine;
using UnityEngine.Rendering;

public class GeneratorTower : FixedTower
{
    GameplayManager gameplayManager;
    public int moneyPerAttack = 10;

    protected override void Start()
    {
        base.Start();
    }


    protected override void Fire(Enemy target)
    {
        gameplayManager.GetMoney(moneyPerAttack);
    }
}
