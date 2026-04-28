using System.Text;
using TMPro;
using UnityEngine;

public class PlayerStatsPanelUI : MonoBehaviour
{
    public GameObject statsPanel;
    public TextMeshProUGUI statsContentText;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ShowPanel();
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            HidePanel();
        }
    }

    void ShowPanel()
    {
        if (statsPanel != null)
        {
            statsPanel.SetActive(true);
            RefreshStats();
        }
    }

    void HidePanel()
    {
        if (statsPanel != null)
        {
            statsPanel.SetActive(false);
        }
    }

    public void RefreshStats()
    {
        if (PlayerStats.Instance == null || statsContentText == null) return;

        Health hp = PlayerStats.Instance.GetComponent<Health>();

        float dmg = PlayerStats.Instance.bulletDamageMultiplier;
        float fire = PlayerStats.Instance.fireRateMultiplier;
        float speed = PlayerStats.Instance.moveSpeedMultiplier;
        int maxHP = hp != null ? hp.maxHP : 0;

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Damage Multiplier: x" + dmg.ToString("F1"));
        sb.AppendLine("Fire Rate Multiplier: x" + fire.ToString("F1"));
        sb.AppendLine("Move Speed Multiplier: x" + speed.ToString("F1"));
        sb.AppendLine("Max HP: " + maxHP);
        sb.AppendLine("");
        sb.AppendLine("Acquired Skills:");
        sb.AppendLine("Dash: " + (PlayerStats.Instance.dashUnlocked ? "Unlocked" : "Locked"));
        sb.AppendLine("Multi Shot: " + (PlayerStats.Instance.multiShotUnlocked ? "Unlocked" : "Locked"));

        var skills = PlayerStats.Instance.GetAcquiredSkills();
        if (skills.Count == 0)
        {
            sb.AppendLine("- None");
        }
        else
        {
            foreach (string skill in skills)
            {
                sb.AppendLine("- " + skill);
            }
        }

        statsContentText.text = sb.ToString();
    }
}