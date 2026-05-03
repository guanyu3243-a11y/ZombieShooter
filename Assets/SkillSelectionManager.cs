using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillSelectionManager : MonoBehaviour
{
    public static SkillSelectionManager Instance;
    public AudioSource audioSource;
    public AudioClip openSkillPanelClip;

    [Header("UI")]
    public GameObject skillPanel;

    public TextMeshProUGUI button1Text;
    public TextMeshProUGUI button2Text;
    public TextMeshProUGUI button3Text;

    private SkillType button1Skill;
    private SkillType button2Skill;
    private SkillType button3Skill;

    void Awake()
    {
        Instance = this;
    }

    public void OpenSmallSkillSelection()
    {
        GenerateSmallSkills();

        if (audioSource != null && openSkillPanelClip != null)
        {
            audioSource.PlayOneShot(openSkillPanelClip);
        }

        if (skillPanel != null)
        {
            skillPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    void GenerateSmallSkills()
    {
        List<SkillType> skillPool = new List<SkillType>
    {
        SkillType.DamageUp,
        SkillType.FireRateUp,
        SkillType.MoveSpeedUp,
        SkillType.CriticalHit,
        SkillType.Heal,
        SkillType.MaxHPUp
    };

        Shuffle(skillPool);

        button1Skill = skillPool[0];
        button2Skill = skillPool[1];
        button3Skill = skillPool[2];

        button1Text.text = GetSkillName(button1Skill);
        button2Text.text = GetSkillName(button2Skill);
        button3Text.text = GetSkillName(button3Skill);
    }
    public void OpenMajorSkillSelection()
    {
        GenerateMajorSkills();
        if (audioSource != null && openSkillPanelClip != null)
            audioSource.PlayOneShot(openSkillPanelClip);
        skillPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void GenerateMajorSkills()
    {
        List<SkillType> majorSkillPool = new List<SkillType>();

        if (!PlayerStats.Instance.multiShotUnlocked)
            majorSkillPool.Add(SkillType.MultiShot);

        if (!PlayerStats.Instance.dashUnlocked)
            majorSkillPool.Add(SkillType.DashUnlock);

        if (!PlayerStats.Instance.enemySlowUnlocked)
            majorSkillPool.Add(SkillType.EnemySlow);

        Shuffle(majorSkillPool);

        button1Skill = majorSkillPool[0];
        button2Skill = majorSkillPool[1];
        button3Skill = majorSkillPool[2];

        button1Text.text = GetSkillName(button1Skill);
        button2Text.text = GetSkillName(button2Skill);
        button3Text.text = GetSkillName(button3Skill);
    }
    string GetSkillName(SkillType skill)
    {
        switch (skill)
        {
            case SkillType.DamageUp:
                return "Damage Up";

            case SkillType.FireRateUp:
                return "Fire Rate Up";

            case SkillType.Heal:
                return "Heal 30 HP";

            case SkillType.MaxHPUp:
                return "Max HP Up";

            case SkillType.MoveSpeedUp:
                return "Move Speed Up";

            case SkillType.DashUnlock:
                return "Unlock Dash";

            case SkillType.MultiShot:
                return "Multi Shot";

            case SkillType.CriticalHit:
                return "Critical Hit";

            case SkillType.EnemySlow:
                return "Enemy Slow";

            default:
                return "Unknown Skill";

        }
    }

    void ApplySkill(SkillType skill)
    {
        if (PlayerStats.Instance == null) return;

        switch (skill)
        {
            case SkillType.DamageUp:
                PlayerStats.Instance.IncreaseDamage(0.2f);
                break;

            case SkillType.FireRateUp:
                PlayerStats.Instance.IncreaseFireRate(0.2f);
                break;

            case SkillType.Heal:
                PlayerStats.Instance.HealPlayer(30);
                break;

            case SkillType.MaxHPUp:
                PlayerStats.Instance.IncreaseMaxHP(20);
                break;

            case SkillType.MoveSpeedUp:
                PlayerStats.Instance.IncreaseMoveSpeed(0.15f);
                break;

            case SkillType.DashUnlock:
                PlayerStats.Instance.UnlockDash();
                break;

            case SkillType.MultiShot:
                PlayerStats.Instance.UnlockMultiShot();
                break;

            case SkillType.CriticalHit:
                PlayerStats.Instance.IncreaseCriticalChance(0.15f);
                break;

            case SkillType.EnemySlow:
                PlayerStats.Instance.UnlockEnemySlow();
                break;
        }
    }

    public void ChooseButton1()
    {
        ApplySkill(button1Skill);
        CloseSkillSelection();
    }

    public void ChooseButton2()
    {
        ApplySkill(button2Skill);
        CloseSkillSelection();
    }

    public void ChooseButton3()
    {
        ApplySkill(button3Skill);
        CloseSkillSelection();
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

    void Shuffle(List<SkillType> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            SkillType temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}