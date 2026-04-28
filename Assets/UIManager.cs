using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject floatingDamageTextPrefab;
    public Canvas canvas;
    public TMP_Text hpText;
    public TMP_Text scoreText;
    public GameObject gameOverText;
    public TextMeshProUGUI floatingMessageText;
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
    public void ShowFloatingMessage(string message)
    {
        if (floatingMessageText == null) return;

        StopCoroutine(nameof(ShowFloatingMessageRoutine));
        StartCoroutine(ShowFloatingMessageRoutine(message));
    }

    private IEnumerator ShowFloatingMessageRoutine(string message)
    {
        floatingMessageText.text = message;
        floatingMessageText.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.6f);

        floatingMessageText.gameObject.SetActive(false);
    }
    public void ShowDamageText(int damage, bool isCritical, Vector3 worldPosition)
    {
        if (floatingDamageTextPrefab == null || canvas == null) return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);

        GameObject obj = Instantiate(floatingDamageTextPrefab, canvas.transform);
        obj.transform.position = screenPos;

        FloatingDamageText floatingText = obj.GetComponent<FloatingDamageText>();
        if (floatingText != null)
        {
            floatingText.Setup(damage, isCritical);
        }
    }
}
