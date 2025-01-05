using System;
using UnityEngine;

[System.Serializable]
public class UpgradePath
{
    public Tower tower;

    public int currentUpgrade1;
    public int currentUpgrade2;


    public Upgrade[] upgrade1;
    public Upgrade[] upgrade2;

    public virtual void ApplyUpgrade(int which)
    {
        if (which == 0)
        {
            if (currentUpgrade1 >= upgrade1.Length) goto end;
            upgrade1[currentUpgrade1++].Apply(tower);
        }
        else
        {
            if (currentUpgrade2 >= upgrade2.Length) goto end;
            upgrade2[currentUpgrade2++].Apply(tower);
        }

        end:
        GameplayManager.instance.DisableUI();
    }

    public Upgrade GetUpgrade1()
    {
        if (currentUpgrade1 == upgrade1.Length) return null;
        return upgrade1[currentUpgrade1];
    }

    public Upgrade GetUpgrade2()
    {
        if (currentUpgrade2 == upgrade2.Length) return null;
        return upgrade2[currentUpgrade2];
    }

    //public static explicit operator UpgradePath(UnityEngine.Object v)
    //{
    //    throw new NotImplementedException();
    //}
}
