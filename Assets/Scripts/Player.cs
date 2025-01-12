using UnityEngine;

public class Player : MonoBehaviour
{
    public int health;
    public int fullHealth;
    public float moveSpeed;
    public int money;
    public int power = 1;
    public FixedTower nearTower;
    public bool nearRadio = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if(other.gameObject.transform.parent.TryGetComponent<FixedTower>(out FixedTower ft))
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
