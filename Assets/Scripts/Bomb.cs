using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float radius;
    public LayerMask layerMask;
    int power;
    public Rigidbody rb;
    ParticleSystem ps;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ps = transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    public void Launch(Vector3 forceDir, float force, int damage)
    {
        power = damage;
        rb.linearVelocity = forceDir;
    }

    private void OnCollisionEnter(Collision collision)
    {
        BlowUp();
    }

    private void BlowUp()
    {
        audioSource.Play();
        rb.isKinematic = true;
        GetComponent<SphereCollider>().enabled = false;

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, layerMask);
        Debug.Log("collidersNB = " + colliders.Length);
        foreach (Collider collider in colliders)
        {
            GameObject obj = collider.gameObject;
            Enemy e = obj.GetComponent<Enemy>();
            e.TakeDamage(power);
        }
        ps.Play(true);
        GetComponent<Renderer>().enabled = false;
        Invoke(nameof(DestroyThis), 2.0f);
    }

    void DestroyThis()
    {
        Destroy(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);

        Gizmos.DrawSphere(transform.position, radius);
    }
}
