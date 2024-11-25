using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // References to UI elements
    public TextMeshProUGUI lifeCounterText;
    public TextMeshProUGUI coinCounterText;
    public TextMeshProUGUI timerText;
    public Slider healthBar; // Use Slider for the health bar
    public TextMeshProUGUI roundsCounterText;

    // Variables to track game state
    private int playerLives = 3;
    private int coins = 0;
    private float timer = 0f;
    private int rounds = 0;
    private float playerHealth = 100f;
    private float maxHealth = 100f;

    void Update()
    {
        // Update the timer every frame
        timer += Time.deltaTime;
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        timerText.text = $"Time: {minutes}:{seconds:D2}";
    }

    // Method to update the player's lives
    public void UpdateLives(int lives)
    {
        playerLives = lives;
        lifeCounterText.text = $"Lives: {playerLives}";
    }

    // Method to update the coin counter
    public void UpdateCoins(int coinCount)
    {
        coins = coinCount;
        coinCounterText.text = $"Coins: {coins}";
    }

    // Method to update the health bar
    public void UpdateHealth(float health)
    {
        playerHealth = health;
        healthBar.value = playerHealth / maxHealth;
    }

    // Method to update the enemy attack rounds counter
    public void UpdateRounds(int round)
    {
        rounds = round;
        roundsCounterText.text = $"Rounds: {rounds}";
    }
}