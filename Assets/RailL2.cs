using System.Collections;
using UnityEngine;

public class RailL2 : MonoBehaviour
{
    int damage;
    public float speed;
    int pierce;
    IEnumerator MoveForward()
    {
        while (true)
        {
            if (pierce == 0) DestroyThis();
            transform.position += transform.forward * speed * Time.deltaTime;
            yield return null;
        }
    }

    public void Launch(Vector3 target, int dmg, int pierce)
    {
        transform.LookAt(target);
        damage = dmg;
        this.pierce = pierce;
        Invoke(nameof(DestroyThis), 7.5f);
        StartCoroutine(MoveForward());
    }

    void DestroyThis()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<EnemyL2>().TakeDamage(damage);
        pierce--;
    }

}
