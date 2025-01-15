using UnityEngine;

[System.Serializable]
public class Upgrade
{
    public string text;
    public int upgradePrice;

    public int powerOffset;
    public int fullAmmoOffset;
    public int ammoOffset;
    public float timeBetweenShotsOffset;
    public float reloadTimeOffset;
    public float explosionRangeOffset;
    public int moneyPerAttackOffset;
    public float projectileScaleOffset;
    public int pierceOffset;
    

    public Color newColor;

    public void Apply(Tower tower)
    {
        tower.power += powerOffset;
        tower.fullAmmo += ammoOffset;
        tower.ammo += ammoOffset;
        tower.timeBetweenShots += timeBetweenShotsOffset;
        tower.reloadTime += ammoOffset;
        tower.projectileScale += projectileScaleOffset;

        if(explosionRangeOffset > 0)
        {
            BombTower bt = (BombTower) tower;
            bt.explosionRange += explosionRangeOffset;
        }
        if(moneyPerAttackOffset > 0)
        {
            GeneratorTower gt = (GeneratorTower) tower;
            gt.moneyPerAttack += moneyPerAttackOffset;
        }
        if(pierceOffset > 0)
        {
            if(tower.TryGetComponent<TurretTower>(out TurretTower tt))
            {
                tt.pierce += pierceOffset;
            }
            else if(tower.TryGetComponent<RailTower>(out RailTower rt))
            {
                rt.pierce += pierceOffset;
            }
        }

        tower.rend.material.color = newColor;
    }
}
