using UnityEngine;

public class Player : MonoBehaviour
{
    public int health;
    public int fullHealth;
    public float moveSpeed;
    public int money;
    public int power = 1;
    public FixedTower nearTower;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<FixedTower>(out FixedTower ft))
        {
            nearTower = ft;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.TryGetComponent<FixedTower>(out FixedTower ft))
        {
            if(ft == nearTower)
            {
                nearTower = null;
            }
        }
    }
}
