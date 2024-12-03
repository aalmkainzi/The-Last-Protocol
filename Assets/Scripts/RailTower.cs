using DG.Tweening;
using UnityEngine;

public class RailTower : FixedTower
{
    public GameObject rail;
    public float railLaunchPower = 1;
    public GameObject railGunHead;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (enemiesInRange.Count > 0)
        {
            Vector3 ePos = enemiesInRange[0].transform.position;

            railGunHead.transform.LookAt(ePos);
            Vector3 headRot = railGunHead.transform.localRotation.eulerAngles;
            headRot.z = headRot.y = 0;
            headRot.x -= 90.0f;
            railGunHead.transform.localRotation = Quaternion.Euler(headRot);
        }
    }

    public float force = 10.0f;
    protected override void Fire(Enemy target)
    {
        // TODO make it shoot from the correct Vec3
        Vector3 projectileRotation = transform.rotation.eulerAngles;
        projectileRotation.x = railGunHead.transform.rotation.eulerAngles.x;
        GameObject newRail = Instantiate(rail, railGunHead.transform.GetChild(0).position, Quaternion.Euler(projectileRotation));

        newRail.GetComponent<Rail>().Launch(transform.forward, power);
    }
}
