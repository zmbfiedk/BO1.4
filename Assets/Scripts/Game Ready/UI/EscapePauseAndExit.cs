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
                // If Escape pressed again within time, load scene 0
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Time.timeScale = 1f; // Make sure time is normal before loading
                    SceneManager.LoadScene(0);
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
