using UnityEngine;
using TMPro;

public class SkillCooldownUI : MonoBehaviour
{
    public PlayerController player;
    public PlayerStats stats;

    public TextMeshProUGUI dashText;
    public TextMeshProUGUI slowText;

    void Update()
    {
        // Dash
        if (player != null && dashText != null && stats != null)
        {
            if (!stats.dashUnlocked)
            {
                dashText.text = "[Shift] Dash: LOCKED";
            }
            else if (player.DashTimer > 0f)
            {
                dashText.text = "[Shift] Dash: " + player.DashTimer.ToString("F1") + "s";
            }
            else
            {
                dashText.text = "[Shift] Dash: READY";
            }
        }

        // Slow
        if (stats != null && slowText != null)
        {
            if (!stats.enemySlowUnlocked)
                slowText.text = "[E] Slow: LOCKED";
            else if (stats.EnemySlowTimer > 0f)
                slowText.text = "[E] Slow: " + stats.EnemySlowTimer.ToString("F1") + "s";
            else
                slowText.text = "[E] Slow: READY";
        }
    }
}