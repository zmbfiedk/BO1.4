using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] Button ButtonFast;
    [SerializeField] Button ButtonNormal;
    [SerializeField] Button ButtonTest;
    [SerializeField] Button ButtonReload; // <-- Added for reloading

    void Start()
    {
        ButtonNormal.onClick.AddListener(OnNormalClick);
        ButtonFast.onClick.AddListener(OnFastClick);
        ButtonTest.onClick.AddListener(OnTestClick);
        ButtonReload.onClick.AddListener(OnReloadClick); // <-- Added
    }

    void OnNormalClick()
    {
        SceneManager.LoadScene(1);
    }

    void OnFastClick()
    {
        SceneManager.LoadScene(2);
    }

    void OnTestClick()
    {
        SceneManager.LoadScene(4);
    }

    void OnReloadClick()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
