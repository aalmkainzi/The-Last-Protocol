using UnityEngine;

public class TowerInteraction : MonoBehaviour
{
    Player player;
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("NEAR PLAYER");
        player.nearRadio = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("NOT NEAR PLAYER");
        player.nearRadio = false;
    }
}
