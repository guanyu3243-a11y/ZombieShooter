using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerBossAI : MonoBehaviour
{
    [Header("Summon Settings")]
    public GameObject enemyPrefab;
    public int summonCount = 3;
    public float baseSummonInterval = 5f;
    public float lowHpSummonInterval = 2.5f;
    public float summonRadius = 4f;

    private Health health;
    private bool isSummoning = true;
    private List<GameObject> summonedEnemies = new List<GameObject>();
    void Start()
    {
        health = GetComponent<Health>();
        StartCoroutine(SummonLoop());
    }

    IEnumerator SummonLoop()
    {
        while (isSummoning)
        {
            float interval = GetCurrentSummonInterval();
            yield return new WaitForSeconds(interval);

            if (health == null || health.currentHP <= 0)
            {
                yield break;
            }

            SummonEnemies();
        }
    }

    float GetCurrentSummonInterval()
    {
        if (health == null) return baseSummonInterval;

        float hpPercent = (float)health.currentHP / health.maxHP;

        if (hpPercent <= 0.3f)
        {
            return lowHpSummonInterval;
        }

        if (hpPercent <= 0.6f)
        {
            return 3.5f;
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

        Debug.Log("Summoner Boss summoned enemies.");
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