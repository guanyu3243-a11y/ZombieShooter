using System.Collections;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("Spawn Around Player")]
    public Transform player;
    public float minSpawnDistance = 10f;
    public float maxSpawnDistance = 16f;

    [Header("Wave Settings")]
    public int currentWave = 0;
    public int baseEnemyCount = 5;
    public float timeBetweenWaves = 3f;

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

        int enemiesToSpawn = baseEnemyCount + (currentWave - 1) * 2;

        if (waveText != null)
        {
            waveText.text = "Wave: " + currentWave;
        }

        StartCoroutine(SpawnWave(enemiesToSpawn));
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

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0 || enemyPrefab == null)
        {
            Debug.LogWarning("Spawn points or enemy prefab not assigned.");
            return;
        }

        Vector3 spawnPosition = GetSpawnPositionAroundPlayer();
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
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
        // ===== 难度成长公式 =====
        int enemyHP = 100 + (currentWave - 1) * 20;
        int enemyDamage = 10 + (currentWave - 1) * 2;
        float enemySpeed = 3f + (currentWave - 1) * 0.2f;
        float attackInterval = Mathf.Max(0.3f, 1f - currentWave * 0.05f); // 越高越快攻击

        // ===== 设置血量 =====
        Health health = enemy.GetComponent<Health>();
        if (health != null)
        {
            health.SetMaxHP(enemyHP);
        }

        // ===== 设置AI属性 =====
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
        yield return new WaitForSeconds(timeBetweenWaves);

        if (currentWave % 3 == 0)
        {
            if (SkillSelectionManager.Instance != null)
            {
                SkillSelectionManager.Instance.OpenSkillSelection();
            }
        }
        else
        {
            StartNextWave();
        }
    }
}