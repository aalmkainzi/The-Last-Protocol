using UnityEngine;

public class TurretTower : FixedTower
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
    protected override void Fire(Enemy target)
    {
        // TODO make it shoot from the correct Vec3
        //Vector3 projectileRotation = railGunHead.transform.rotation.eulerAngles;
        //projectileRotation.x += -90.0f;
        GameObject newRail = Instantiate(rail, bulletSpawn.position, Quaternion.identity);//Quaternion.Euler(projectileRotation));

        newRail.GetComponent<Rail>().Launch(target.transform.position, power, pierce);
    }
}
