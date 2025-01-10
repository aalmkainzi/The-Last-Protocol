using UnityEngine;

public class BombTower : FixedTower
{
    public GameObject bomb;
    public float explosionRange = 0.5f;
    public float bombThrowPower = 1;
    public float force = 10.0f;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }


    public Vector3 GetVelocityOfProjectile(Vector3 target, float timeToReach)
    {
        float gravity = -Physics.gravity.y;
        float launchAngle = 45.0f;

        Vector3 launchPosition = transform.position; // Current position of the catapult (the launcher)
        Vector3 displacement = target - launchPosition; // The displacement vector from launch to target

        // Calculate horizontal distance (ignore y-axis)
        float horizontalDistance = new Vector3(displacement.x, 0, displacement.z).magnitude;

        // Calculate the vertical distance (height difference between target and launch point)
        float verticalDistance = displacement.y;

        // Convert launch angle to radians
        float launchAngleRad = launchAngle * Mathf.Deg2Rad;

        // Calculate the horizontal velocity component (vX)
        float vX = Mathf.Sqrt(gravity * horizontalDistance / Mathf.Sin(2 * launchAngleRad));

        // Calculate the vertical velocity component (vY)
        float vY = Mathf.Tan(launchAngleRad) * vX;

        // Calculate the velocity components in the x and z directions (normalized)
        Vector3 horizontalVelocity = new Vector3(displacement.x, 0, displacement.z).normalized * vX;

        // Calculate the final vertical velocity component
        Vector3 velocity = horizontalVelocity + Vector3.up * vY;
        velocity.y *= 0.95f;
        return velocity;
    }
    protected override void Fire(Enemy target)
    {
        GameObject newBomb = Instantiate(bomb, transform.position + new Vector3(0, 0.7f, 0), Quaternion.identity);
        newBomb.transform.localScale = new Vector3(projectileScale, projectileScale, projectileScale);
        newBomb.GetComponent<Bomb>().radius = explosionRange;

        newBomb.GetComponent<Bomb>().Launch(GetVelocityOfProjectile(target.transform.position, 1.0f), force, power);

    }
}
