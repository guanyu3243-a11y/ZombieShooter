using UnityEngine;

public class SkillSelectionManager : MonoBehaviour
{
    public static SkillSelectionManager Instance;

    public GameObject skillPanel;

    void Awake()
    {
        Instance = this;
    }

    public void OpenSkillSelection()
    {
        if (skillPanel != null)
        {
            skillPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void CloseSkillSelection()
    {
        if (skillPanel != null)
        {
            skillPanel.SetActive(false);
            Time.timeScale = 1f;
        }

        WaveManager waveManager = FindFirstObjectByType<WaveManager>();
        if (waveManager != null)
        {
            waveManager.StartNextWave();
        }
    }

    public void ChooseDamageUpgrade()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.IncreaseDamage(0.2f);
        }

        CloseSkillSelection();
    }

    public void ChooseFireRateUpgrade()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.IncreaseFireRate(0.2f);
        }

        CloseSkillSelection();
    }

    public void ChooseHeal()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.HealPlayer(30);
        }

        CloseSkillSelection();
    }
}