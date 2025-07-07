using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapePauseAndExit : MonoBehaviour
{
    private int escapePressCount = 0;
    private float timer = 0f;
    private float maxTimeBetweenPresses = 1.5f; // seconds
    private bool isPaused = false;

    void Update()
    {
        if (!isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                escapePressCount = 1;
                PauseGame();
                timer = 0f;
            }
        }
        else
        {
            timer += Time.unscaledDeltaTime;

            if (escapePressCount == 1)
            {
                // Escape pressed again within time, exit to main menu
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Time.timeScale = 1f; // Reset time
                    SceneManager.LoadScene(0);
                }
                // Press "R" to reload current scene
                else if (Input.GetKeyDown(KeyCode.R))
                {
                    Time.timeScale = 1f; // Reset time
                    Scene currentScene = SceneManager.GetActiveScene();
                    SceneManager.LoadScene(currentScene.name);
                }
                else if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape))
                {
                    // Any other key pressed unpauses
                    ResumeGame();
                    escapePressCount = 0;
                }
                else if (timer > maxTimeBetweenPresses)
                {
                    // Timeout, unpause
                    ResumeGame();
                    escapePressCount = 0;
                }
            }
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        // Optional: show pause menu here
    }

    void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        // Optional: hide pause menu here
    }
}
