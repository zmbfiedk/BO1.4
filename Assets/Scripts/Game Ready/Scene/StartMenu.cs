using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] Button ButtonFast;
    [SerializeField] Button ButtonNormal;
    [SerializeField] Button ButtonTutorials;
    void Start()
    {
        ButtonNormal.onClick.AddListener(OnNormalClick);
        ButtonFast.onClick.AddListener(OnFastClick);
        ButtonTutorials.onClick.AddListener(OnTutorialClick);
    }

    void OnNormalClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
    void OnFastClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
    }
    void OnTutorialClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}