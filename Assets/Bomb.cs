using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float radius;
    int layerMask;
    int power;
    public Rigidbody rb;
    void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("EnemyBot");
    }

    void Update()
    {
        
    }

    public void Launch(Vector3 forceDir, float force, int damage)
    {
        power = damage;
        rb.AddForce(forceDir * force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        BlowUp();
    }

    private void BlowUp()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, layerMask);

        foreach (Collider collider in colliders)
        {
            GameObject obj = collider.gameObject;
            Enemy e = obj.GetComponent<Enemy>();
            e.TakeDamage(power);
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);

        Gizmos.DrawSphere(transform.position, radius);
    }
}
