using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RadarTower : MonoBehaviour
{
    public int fullHealth;
    public int health;
    public Player player;

    public GameplayManager gameplayManager;
    // player can upgrade his weapon at the radar tower

    Slider healthSlider;
    void Start()
    {
        healthSlider = GameObject.FindWithTag("HealthSlider").GetComponent<Slider>();
        healthSlider.value = 0;
        gameplayManager = GameObject.FindWithTag("gameplayManager").GetComponent<GameplayManager>();
        player = GameObject.FindWithTag("player").GetComponent<Player>();
    }

    void Update()
    {
        
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        healthSlider.value = 1.0f - ((float) health / fullHealth);
        if (health <= 0)
        {
            gameplayManager.Lose();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        player.moveSpeed += 2.0f;
    }
}
