using UnityEngine;

public class TurretTowerL2 : FixedTowerL2
{
    public GameObject rail;
    public float railLaunchPower = 1;
    public int pierce;
    public Transform bulletSpawn;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public float force = 10.0f;
    protected override void Fire(EnemyL2 target)
    {
        GameObject newRail = Instantiate(rail, bulletSpawn.position, Quaternion.identity);

        newRail.GetComponent<Rail>().Launch(target.transform.position, power, pierce);
    }
}
