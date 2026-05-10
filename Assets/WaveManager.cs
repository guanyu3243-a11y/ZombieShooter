using System.Collections;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("Boss Settings")]
    public GameObject heavyBossPrefab;
    public GameObject summonerBossPrefab;

    [Header("Spawn Around Player")]
    public Transform player;
    public float minSpawnDistance = 10f;
    public float maxSpawnDistance = 16f;

    [Header("Wave Settings")]
    public int currentWave = 0;
    public int baseEnemyCount = 5;
    public float normalWaveDelay = 3f;
    public float bossWaveDelay = 6f;

    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    [Header("UI")]
    public TextMeshProUGUI waveText;

    private int enemiesAlive = 0;
    private bool waveInProgress = false;

    void Start()
    {
        StartNextWave();
    }

    public void StartNextWave()
    {
        currentWave++;
        waveInProgress = true;

        if (waveText != null)
        {
            waveText.text = "Wave: " + currentWave;
        }
        if (IsBossWave(currentWave))
        {
            SpawnBossWave();
            return;
        }
        int enemiesToSpawn = baseEnemyCount + Mathf.FloorToInt((currentWave - 1) * 1.2f);
        StartCoroutine(SpawnWave(enemiesToSpawn));
    }
    bool IsBossWave(int wave)
    {
        return wave % 10 == 0;
    }
    void SpawnBossWave()
    {
        enemiesAlive = 1;

        GameObject bossPrefab = null;

        if (currentWave == 10)
        {
            bossPrefab = heavyBossPrefab;
        }
        else if (currentWave == 20)
        {
            bossPrefab = summonerBossPrefab;
        }
        else
        {
            bossPrefab = Random.value < 0.5f ? heavyBossPrefab : summonerBossPrefab;
        }

        Vector3 spawnPosition = GetSpawnPositionAroundPlayer();
        GameObject boss = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);

        HeavyBossAI heavy = boss.GetComponent<HeavyBossAI>();
        if (heavy != null)
        {
            int bossLevel = Mathf.Max(0, currentWave/10 - 1);

            heavy.attackDamage = 30 + bossLevel * 10;
            heavy.attackCooldown = Mathf.Max(1f, 2f - bossLevel * 0.03f);

            heavy.dashDamage = 50 + bossLevel * 15;
            heavy.dashCooldown = Mathf.Max(3f, 6f - bossLevel * 0.05f);
            heavy.dashSpeed = 15 + bossLevel * 1.5f;
            heavy.dashDuration = Mathf.Min(1.2f, 0.4f + bossLevel * 0.06f);
        }

        SummonerBossAI summoner = boss.GetComponent<SummonerBossAI>();
        if (summoner != null)
        {
            int bossLevel = Mathf.Max(0, currentWave / 10 - 1);

            summoner.attackDamage = 20 + bossLevel * 5;
            summoner.attackCooldown = Mathf.Max(1.2f, 2f - bossLevel * 0.1f);

            summoner.summonCount = 2 + bossLevel;
            summoner.baseSummonInterval = Mathf.Max(3f, 5f - bossLevel * 0.3f);
            summoner.lowHpSummonInterval = Mathf.Max(2f, 2.5f - bossLevel * 0.2f);
        }

        Health bossHealth = boss.GetComponent<Health>();
        if (bossHealth != null && BossHealthBarUI.Instance != null)
        {
            BossHealthBarUI.Instance.ShowBossHealth(bossHealth, boss.name);
        }
        if (heavy != null)
        {
            bossHealth.SetMaxHP(500 + currentWave * 60);
        }

        if (summoner != null)
        {
            bossHealth.SetMaxHP(350 + currentWave * 45);
        }
        Debug.Log("Boss Wave: " + currentWave);
    }

    IEnumerator SpawnWave(int enemyCount)
    {
        enemiesAlive = enemyCount;

        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
    }
    Vector3 GetSpawnPositionAroundPlayer()
    {
        if (player == null)
        {
            Debug.LogWarning("Player not assigned in WaveManager.");
            return Vector3.zero;
        }

        Vector2 randomCircle = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(minSpawnDistance, maxSpawnDistance);

        Vector3 spawnOffset = new Vector3(
            randomCircle.x * randomDistance,
            0f,
            randomCircle.y * randomDistance
        );

        return player.position + spawnOffset;
    }
    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0 || enemyPrefab == null)
        {
            Debug.LogWarning("Spawn points or enemy prefab not assigned.");
            return;
        }

        Vector3 spawnPosition = GetSpawnPositionAroundPlayer();
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

      
        // ===== Difficulty Growth Formula =====
        int enemyHP = 50 + (currentWave - 1) * 10;
        int enemyDamage = 10 + (currentWave - 1);
        float enemySpeed = 3f + (currentWave - 1) * 0.2f;
        float attackInterval = Mathf.Max(0.3f, 1f - currentWave * 0.05f); // 越高越快攻击

        // ===== Set Health =====
        Health health = enemy.GetComponent<Health>();
        if (health != null)
        {
            health.SetMaxHP(enemyHP);
        }

        // ===== Set AI property =====
        EnemyAI ai = enemy.GetComponent<EnemyAI>();
        if (ai != null)
        {
            ai.moveSpeed = enemySpeed;
            ai.damagePerTick = enemyDamage;
            ai.tickInterval = attackInterval;
        }

        Debug.Log("Wave " + currentWave +
            " Enemy → HP:" + enemyHP +
            " DMG:" + enemyDamage +
            " SPD:" + enemySpeed +
            " ATK:" + attackInterval);
    }

    public void EnemyKilled()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0 && waveInProgress)
        {
            waveInProgress = false;
            StartCoroutine(BeginNextWaveAfterDelay());
        }
    }

    IEnumerator BeginNextWaveAfterDelay()
    {
        float delay = IsBossWave(currentWave) ? bossWaveDelay : normalWaveDelay;
        yield return new WaitForSeconds(delay);

        // Boss前一波
        if ((currentWave + 1) % 10 == 0)
        {
            if (SkillSelectionManager.Instance.HasMajorSkillsLeft())
                SkillSelectionManager.Instance.OpenMajorSkillSelection();
            else
                StartNextWave();
        }
        else if (currentWave % 2 == 0)
        {
            SkillSelectionManager.Instance.OpenSmallSkillSelection();
        }
        else
        {
            StartNextWave();
        }
    }
       
    }
