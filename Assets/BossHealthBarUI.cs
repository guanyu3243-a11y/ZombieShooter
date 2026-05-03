using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBarUI : MonoBehaviour
{
    public static BossHealthBarUI Instance;

    public Slider bossHealthSlider;
    public TextMeshProUGUI bossNameText;

    private Health currentBossHealth;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowBossHealth(Health bossHealth, string bossName)
    {
        currentBossHealth = bossHealth;

        if (bossNameText != null)
        {
            bossNameText.text = bossName;
        }

        bossHealthSlider.maxValue = bossHealth.maxHP;
        bossHealthSlider.value = bossHealth.currentHP;

        gameObject.SetActive(true);
    }

    public void HideBossHealth()
    {
        currentBossHealth = null;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (currentBossHealth == null) return;

        bossHealthSlider.maxValue = currentBossHealth.maxHP;
        bossHealthSlider.value = currentBossHealth.currentHP;

        if (currentBossHealth.currentHP <= 0)
        {
            HideBossHealth();
        }
    }
}