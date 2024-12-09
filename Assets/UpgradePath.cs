using UnityEngine;

public class UpgradePath
{
    public Tower tower;

    Upgrade upgrade1;
    Upgrade upgrade2;

    public void ApplyUpgrade(int which)
    {
        Upgrade u;
        if (which == 0)
        {
            u = upgrade1;
            if(upgrade1.next != null)
            {
                upgrade1 = (Upgrade)upgrade1.next;
            }
        }
        else
        {
            u = upgrade2;
        }

        tower.power += u.powerOffset;
        tower.fullAmmo += u.ammoOffset;
        tower.ammo += u.ammoOffset;
        tower.timeBetweenShots += u.timeBetweenShotsOffset;
        tower.reloadTime += u.ammoOffset;

        tower.gameObject.GetComponent<Renderer>().material.color = u.newColor;

        GameObject.FindWithTag("gameplayManager").GetComponent<GameplayManager>().DisableUI();
    }
}
