using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI Panelleri")]
    public GameObject gameOverPanel;

    [Header("Butonlar")]
    public Button restartButton;
    public Button mainMenuButton;

    [Header("Sahneler")]
    public string mainMenuSceneName = "MainMenu"; // menu sahnenin adi

    // oyun bitti mi
    private bool isGameOver = false;

    void Start()
    {
        // baslangicta game over paneli kapali
        gameOverPanel.SetActive(false);

        // butonlara fonksiyon bagla
        restartButton.onClick.AddListener(RestartGame);
        mainMenuButton.onClick.AddListener(GoToMainMenu);

        // oyun baslarken zaman normal
        Time.timeScale = 1f;
    }

    // bu fonksiyonu PlayerController cagiracak
    public void GameOver()
    {
        if (isGameOver) return; // zaten bittiyse tekrar calismasin

        isGameOver = true;

        // oyunu durdur
        Time.timeScale = 0f;

        // game over paneli ac
        gameOverPanel.SetActive(true);
        FindFirstObjectByType<ScoreManager>()?.SaveHighScore();
    }

    // yeniden baslat
    void RestartGame()
    {
        Time.timeScale = 1f; // zamani sifirla
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // sahneyi yeniden yukle
    }

    // main menuye don
    void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}