using System.Collections;
using UnityEngine;

public class Rail : MonoBehaviour
{
    int damage;
    public float speed;
    IEnumerator MoveForward()
    {
        while(true)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
            yield return null;
        }
    }

    public void Launch(Vector3 dir, int dmg)
    {
        damage = dmg;
        Invoke(nameof(DestroyThis), 7.5f);
        StartCoroutine(MoveForward());
    }

    void DestroyThis()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
    }

}
