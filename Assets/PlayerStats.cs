using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip enemySlowClip;

    [Header("Player Stats")]
    public float bulletDamageMultiplier = 1f;
    public float fireRateMultiplier = 1f;
    public float moveSpeedMultiplier = 1f;
    public bool dashUnlocked = false;
    public bool multiShotUnlocked = false;
    public bool enemySlowUnlocked = false;
    public float critChance = 0f;
    public float critMultiplier = 2f;
    public float enemySlowMultiplier = 0.4f; // 敌人变成40%速度
    public float enemySlowDuration = 5f;
    public float enemySlowCooldown = 5f;
    private float enemySlowTimer = 0f;

    public float EnemySlowTimer => enemySlowTimer;
    public float EnemySlowCooldown => enemySlowCooldown;
    private bool enemySlowReady = true;

    private List<string> acquiredSkills = new List<string>();

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Instance = this;
    }

    public void IncreaseDamage(float amount)
    {
        bulletDamageMultiplier += amount;
        AddSkillRecord("Damage Up");
        Debug.Log("Damage Multiplier: " + bulletDamageMultiplier);
    }

    public void IncreaseFireRate(float amount)
    {
        fireRateMultiplier += amount;
        AddSkillRecord("Fire Rate Up");
        Debug.Log("Fire Rate Multiplier: " + fireRateMultiplier);
    }

    public void IncreaseMoveSpeed(float amount)
    {
        moveSpeedMultiplier += amount;
        AddSkillRecord("Move Speed Up");
        Debug.Log("Move Speed Multiplier: " + moveSpeedMultiplier);
    }

    public void IncreaseMaxHP(int amount)
    {
        Health hp = GetComponent<Health>();
        if (hp != null)
        {
            hp.maxHP += amount;
            hp.currentHP += amount;
            AddSkillRecord("Max HP Up");

            if (UIManager.Instance != null)
            {
                UIManager.Instance.SetHP(hp.currentHP);
            }

            Debug.Log("Max HP increased to: " + hp.maxHP);
        }
    }

    public void HealPlayer(int healAmount)
    {
        Health hp = GetComponent<Health>();
        if (hp != null)
        {
            hp.currentHP = Mathf.Min(hp.currentHP + healAmount, hp.maxHP);
            AddSkillRecord("Heal 30 HP");

            if (UIManager.Instance != null)
            {
                UIManager.Instance.SetHP(hp.currentHP);
            }

            Debug.Log("Player healed to: " + hp.currentHP);
        }
    }
    public void IncreaseCriticalChance(float amount)
    {
        critChance += amount;
        AddSkillRecord("Critical Hit Chance Up");
        Debug.Log("Crit Chance: " + critChance);
    }
    public void UnlockDash()
    {
        dashUnlocked = true;
        AddSkillRecord("Dash Unlock");
        Debug.Log("Dash Unlocked");
    }

    public void UnlockMultiShot()
    {
        multiShotUnlocked = true;
        AddSkillRecord("Multi Shot");
        Debug.Log("Multi Shot Unlocked");
    }
    public void UnlockEnemySlow()
    {
        enemySlowUnlocked = true;
        AddSkillRecord("Enemy Slow");
        Debug.Log("Enemy Slow Unlocked");
    }
    public bool TryUseEnemySlow()
    {
        if (!enemySlowUnlocked) return false;
        if (!enemySlowReady) return false;
        if (enemySlowTimer > 0) return false;

        StartCoroutine(EnemySlowRoutine());
        enemySlowTimer = enemySlowCooldown;

        if (audioSource != null && enemySlowClip != null)
        {
            audioSource.PlayOneShot(enemySlowClip);
        }

        return true;
    }
    void Update()
    {
        if (enemySlowTimer > 0f)
        {
            enemySlowTimer -= Time.deltaTime;
        }
    }
    private IEnumerator EnemySlowRoutine()
    {
        enemySlowReady = false;

        EnemyAI[] enemies = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);

        foreach (EnemyAI enemy in enemies)
        {
            enemy.moveSpeed *= enemySlowMultiplier;
        }

        yield return new WaitForSeconds(enemySlowDuration);

        foreach (EnemyAI enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.moveSpeed /= enemySlowMultiplier;
            }
        }

        yield return new WaitForSeconds(enemySlowCooldown);

        enemySlowReady = true;
    }
    void AddSkillRecord(string skillName)
    {
        acquiredSkills.Add(skillName);
    }

    public List<string> GetAcquiredSkills()
    {
        return acquiredSkills;
    }
}