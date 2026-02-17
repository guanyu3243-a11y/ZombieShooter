using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TMP_Text hpText;
    public TMP_Text scoreText;
    public GameObject gameOverText;

    private int score = 0;

    void Awake()
    {
        Instance = this;
        if (gameOverText != null) gameOverText.SetActive(false);
        UpdateScore(0);
    }

    public void SetHP(int hp)
    {
        if (hpText != null) hpText.text = "HP: " + hp;
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScore(score);
    }

    void UpdateScore(int value)
    {
        if (scoreText != null) scoreText.text = "Score: " + value;
    }

    public void ShowGameOver()
    {
        if (gameOverText != null) gameOverText.SetActive(true);
    }
}
