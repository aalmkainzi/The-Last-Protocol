using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public GameObject pauseMenuUI;  // Reference to the Pause Menu UI
    private bool isPaused = false;  // Is the game paused?

    void Update()
    {
        // Toggle pause on/off when Esc is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        // If the game is paused, check for M key to return to main menu
        if (isPaused && Input.GetKeyDown(KeyCode.M))
        {
            GoToMainMenu();
        }
    }

    // Function to resume the game
    public void Resume()
    {
        pauseMenuUI.SetActive(false);  // Hide the pause menu UI
        Time.timeScale = 1f;  // Set timeScale back to 1 to resume the game
        isPaused = false;
    }

    // Function to pause the game
    public void Pause()
    {
        pauseMenuUI.SetActive(true);  // Show the pause menu UI
        Time.timeScale = 0f;  // Freeze time to pause the game
        isPaused = true;
    }

    // Function to quit to the main menu
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;  // Reset timeScale to normal before quitting
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainMenu");  // Load the main menu scene
    }
}