using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] Button ButtonFast;
    [SerializeField] Button ButtonNormal;
    [SerializeField] Button ButtonTutorials;
    [SerializeField] Button ButtonTest;
    [SerializeField] Button ButtonReload; // <-- Added for reloading

    void Start()
    {
        ButtonNormal.onClick.AddListener(OnNormalClick);
        ButtonFast.onClick.AddListener(OnFastClick);
        ButtonTutorials.onClick.AddListener(OnTutorialClick);
        ButtonTest.onClick.AddListener(OnTestClick);
        ButtonReload.onClick.AddListener(OnReloadClick); // <-- Added
    }

    void OnNormalClick()
    {
        SceneManager.LoadScene(2);
    }

    void OnFastClick()
    {
        SceneManager.LoadScene(3);
    }

    void OnTutorialClick()
    {
        SceneManager.LoadScene(1);
    }

    void OnTestClick()
    {
        SceneManager.LoadScene(5);
    }

    void OnReloadClick()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
