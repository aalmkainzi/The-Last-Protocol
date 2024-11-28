using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject uiPanel; // Reference to the Panel containing the UI
    private bool isNearPoint = false; // Tracks if the player is near the interaction point
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
        if (isNearPoint && Input.GetKeyDown(KeyCode.E))
        {
            ToggleUI();
        }
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
    
    public void ToggleUI()
    {
        // Enable/Disable the UI Panel
        uiPanel.SetActive(!uiPanel.activeSelf);

        // Pause the game when the UI is active
        if (uiPanel.activeSelf)
        {
            Time.timeScale = 0; // Pause time
        }
        else
        {
            Time.timeScale = 1; // Resume time
        }
    }
    // Call this when the player enters the interaction point
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearPoint = true;
        }
    }

    // Call this when the player leaves the interaction point
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearPoint = false;
        }
    }

    // Functions to call when buttons are clicked
    public void SelectCannon()
    {
        Debug.Log("Cannon Selected!");
        CloseUI();
    }

    public void SelectArc()
    {
        Debug.Log("Arc Selected!");
        CloseUI();
    }

    public void SelectWarrior()
    {
        Debug.Log("Warrior Selected!");
        CloseUI();
    }

    private void CloseUI()
    {
        uiPanel.SetActive(false);
        Time.timeScale = 1; // Resume time
    }
}