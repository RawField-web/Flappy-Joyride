using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    // anlik skor
    private int score = 0;

    void Start()
    {
        // baslangicta skor sifir
        score = 0;
        UpdateUI();
    }

    public void AddScore()
    {
        score++;
        UpdateUI();
    }

    // ekrani guncelle
    void UpdateUI()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        scoreText.text = "Skor: " + score;
        highScoreText.text = "En Yuksek: " + highScore;
    }

    // oyun bitince skoru kaydet
    public void SaveHighScore()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
        }
    }
}