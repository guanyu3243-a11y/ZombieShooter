using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerBossAI : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip summonClip;
    public AudioClip attackClip;
    public AudioClip deathClip;

    [Header("Summon Settings")]
    public GameObject enemyPrefab;
    public int summonCount = 3;
    public float baseSummonInterval = 5f;
    public float lowHpSummonInterval = 2.5f;
    public float summonRadius = 4f;

    [Header("Attack Settings")]
    public float attackRange = 2.5f;
    public int attackDamage = 20;
    public float attackCooldown = 1.5f;

    private Transform player;
    private float attackTimer;
    private Animator anim;
    private Health health;
    private bool isSummoning = true;
    private bool isCasting = false;
    private List<GameObject> summonedEnemies = new List<GameObject>();
    private const string KEY_SFX = "SETTINGS_SFX";

    private float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat(KEY_SFX, 1f);
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        health = GetComponent<Health>();
        anim = GetComponent<Animator>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;
        }

        StartCoroutine(SummonLoop());
    }
    void Update()
    {
        HandleAttack();
    }
    
    IEnumerator SummonLoop()
    {
        while (isSummoning)
        {
            float interval = GetCurrentSummonInterval();
            yield return new WaitForSeconds(interval);

            if (health == null || health.currentHP <= 0)
            {
                if (audioSource != null && deathClip != null)
                    audioSource.PlayOneShot(deathClip, GetSFXVolume());

                yield break;
            }

            StartCoroutine(PlaySummonAnimation());
        }
    }
    IEnumerator PlaySummonAnimation()
    {
        isCasting = true;
        attackTimer = attackCooldown;

        if (anim != null)
        {
            anim.ResetTrigger("Attack");
            anim.SetTrigger("Summon");
        }
        if (audioSource != null && summonClip != null)
        {
            audioSource.PlayOneShot(summonClip, GetSFXVolume());
        }

        yield return new WaitForSeconds(0.8f);

        SummonEnemies();

        yield return new WaitForSeconds(1.0f);

        isCasting = false;
    }
    float GetCurrentSummonInterval()
    {
        if (health == null) return baseSummonInterval;

        float hpPercent = (float)health.currentHP / health.maxHP;

        if (hpPercent <= 0.3f)
        {
            return lowHpSummonInterval;
        }


        return baseSummonInterval;
    }

    void SummonEnemies()
    {
        if (enemyPrefab == null) return;

        for (int i = 0; i < summonCount; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle.normalized;
            Vector3 offset = new Vector3(randomCircle.x, 0f, randomCircle.y) * summonRadius;

            Vector3 spawnPos = transform.position + offset;

            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            summonedEnemies.Add(enemy);
        }

    }
    void HandleAttack()
    {
        if (isCasting) return;
        if (player == null) return;
        if (health != null && health.currentHP <= 0) return;

        attackTimer -= Time.deltaTime;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange && attackTimer <= 0f)
        {
            attackTimer = attackCooldown;

            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }

            if (anim != null)
            {
                anim.SetTrigger("Attack");
            }

        }
    }
    public void ClearSummonedEnemies()
    {
        foreach (GameObject enemy in summonedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        summonedEnemies.Clear();
    }
}