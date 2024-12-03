using UnityEngine;

public class BombTower : FixedTower
{
    public GameObject bomb;
    public float bombThrowPower = 1;
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
        GameObject newBomb = Instantiate(bomb, transform.position, Quaternion.identity);

        float dist = Vector3.Distance(transform.position, target.transform.position);
        newBomb.GetComponent<Bomb>().Launch(transform.forward * (dist / 10) + Vector3.up * 1.0f, force, power);
    }
}
