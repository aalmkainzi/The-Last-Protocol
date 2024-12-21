using UnityEngine;

[System.Serializable]
public class Upgrade
{
    public string text;
    
    public int powerOffset;
    public int fullAmmoOffset;
    public int ammoOffset;
    public float timeBetweenShotsOffset;
    public float reloadTimeOffset;
    public float explosionRangeOffset;
    public int moneyPerAttackOffset;
    public float projectileScaleOffset;

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

        tower.rend.material.color = newColor;
    }
}
