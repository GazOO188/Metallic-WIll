using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartGame : MonoBehaviour
{
    //GOALS OF THE SCRIPT: 

    //1) RESTART THE GAME//
    
    //2) QUIT BACK TO THE MAIN MENU


    // UI Elements
    public GameObject RestartButton;
    public GameObject MainMenuButton;
    public TextMeshProUGUI GameOverText;

    // Player references
    public PlayerHealth PH;
    public PlayerController PC;

    // Flag to prevent multiple triggers
    private bool gameOverTriggered = false;

    void Start()
    {
        // Hide Game Over UI at start
        GameOverText.enabled = false;
        RestartButton.SetActive(false);
        MainMenuButton.SetActive(false);

        // Make sure time is running
        Time.timeScale = 1f;
    }

    void Update()
    {
        // Check for player death
        if (PH.currentHealth <= 0 && !gameOverTriggered)
        {
            gameOverTriggered = true;

            // Show Game Over UI
            GameOverText.enabled = true;
            RestartButton.SetActive(true);
            MainMenuButton.SetActive(true);

            // Stop player movement
            PC.CanMove = false;

            // Pause the game
            Time.timeScale = 0f;
        }
    }

    // Restart the current level
    public void RestartFromLevelOne()
    {
        Time.timeScale = 1f; // Reset time scale
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Go back to main menu (scene index 0)
    public void GoBacktoMainMenu()
    {
        Time.timeScale = 1f; // Reset time scale
        SceneManager.LoadScene(0);
    }
}