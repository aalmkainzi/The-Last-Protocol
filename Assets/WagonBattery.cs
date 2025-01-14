using UnityEngine;
using UnityEngine.Rendering;

public class WagonBattery : MonoBehaviour
{
    Player player;
    bool playerNear = false;
    Animator anim;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if(playerNear && !player.holdingBattery)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                player.holdingBattery = true;
                anim.SetTrigger("Pickup");
                Invoke(nameof(DestroyThis), 1.8f);
            }
        }
    }

    void DestroyThis()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        playerNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        playerNear = false;
    }
}
