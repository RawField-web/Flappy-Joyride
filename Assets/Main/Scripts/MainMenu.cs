using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Butonlar")]
    public Button playButton;
    public Button quitButton;

    [Header("Sahneler")]
    public string gameSceneName = "Main";

    void Start()
    {
        playButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
        Time.timeScale = 1f;
    }

    void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    void QuitGame()
    {
        Application.Quit();
    }
}