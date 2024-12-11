using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine.PlayerLoop;


public class DefeatedMenuController : MonoBehaviour
{
    public GameObject defeatedPanel;  // The Defeated UI Panel
    public TMP_Text defeatedText;         // The "You Defeated" text
    public Button restartButton;      // Restart button
    public Button mainMenuButton;     // Main Menu button
    public float textMoveDuration = 1f;  // Time for the text to move

    private bool isPlayerDefeated = false;


    public void Update()
    {
        // Test defeated menu by pressing 'H'
        if (Input.GetKeyDown(KeyCode.H) && !isPlayerDefeated)
        {

            OnPlayerDefeated();
        }
    }

    // Call this method when the player dies
    public void OnPlayerDefeated()
    {
        defeatedPanel.SetActive(true);    // Show the Defeated UI
        StartCoroutine(AnimateDefeatedText());
    }

    // Coroutine to animate the "You Defeated" text moving from center to top
    private IEnumerator AnimateDefeatedText()
    {
        RectTransform textRect = defeatedText.GetComponent<RectTransform>();
        Vector2 startPos = textRect.anchoredPosition;  // Current position (center)
        Vector2 targetPos = new Vector2(startPos.x, Screen.height / 2 - 300);  // Top position
        

        float time = 0;
        while (time < textMoveDuration)
        {
            textRect.anchoredPosition = Vector2.Lerp(startPos, targetPos, time / textMoveDuration);
            time += Time.deltaTime;
            defeatedText.color = Color.Lerp(Color.red, Color.black, time / textMoveDuration);
            yield return null;
        }
        textRect.anchoredPosition = targetPos;

        // Once the text has moved to the top, show the buttons
        ShowButtons();
        // Time.timeScale = 0f;  // Freeze time to pause the game
        // Cursor.lockState = CursorLockMode.None;

    }

    // Show the restart and main menu buttons
    private void ShowButtons()
    {
        restartButton.gameObject.SetActive(true);
        mainMenuButton.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        // Assign actions to buttons
        restartButton.onClick.AddListener(RestartLevel);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    // Function to restart the current level
    private void RestartLevel()
    {
        Time.timeScale = 1f;  // Ensure the game is running
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reload current scene
    }

    // Function to go back to the main menu
    private void GoToMainMenu()
    {
        Time.timeScale = 1f;  // Ensure the game is running
        SceneManager.LoadScene("MainMenu");  // Load main menu scene
    }
}
