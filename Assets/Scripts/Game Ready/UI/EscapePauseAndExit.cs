using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapePauseAndExit : MonoBehaviour
{
    public GameObject pauseOverlay; // Drag your PauseOverlay panel here in Inspector

    private int escapePressCount = 0;
    private float timer = 0f;
    private float maxTimeBetweenPresses = 1.5f;
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
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Time.timeScale = 1f;
                    SceneManager.LoadScene(0);
                }
                else if (Input.GetKeyDown(KeyCode.R))
                {
                    Time.timeScale = 1f;
                    Scene currentScene = SceneManager.GetActiveScene();
                    SceneManager.LoadScene(currentScene.name);
                }
                else if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape))
                {
                    ResumeGame();
                    escapePressCount = 0;
                }
                else if (timer > maxTimeBetweenPresses)
                {
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
        if (pauseOverlay != null)
            pauseOverlay.SetActive(true);
    }

    void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        if (pauseOverlay != null)
            pauseOverlay.SetActive(false);
    }
}
