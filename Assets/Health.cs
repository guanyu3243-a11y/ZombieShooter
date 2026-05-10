using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;
    private Animator anim;
    private bool isDead = false;
    private Renderer[] renderers;
    private Color[] originalColors;
    private bool isFlashing = false;

    void Awake()
    {
        currentHP = maxHP;
        anim = GetComponent<Animator>();

        renderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_Color"))
            {
                originalColors[i] = renderers[i].material.color;
            }
        }
        if (CompareTag("Player") && UIManager.Instance != null)
        {
            UIManager.Instance.SetHP(currentHP);
        }
    }
    public void SetMaxHP(int newMaxHP)
    {
        maxHP = newMaxHP;
        currentHP = newMaxHP;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        currentHP -= amount;
        currentHP = Mathf.Max(0, currentHP);

        if (!CompareTag("Player") && anim != null)
        {
            anim.SetTrigger("Hit");
        }
        if (!CompareTag("Player"))
        {
            StartCoroutine(HitFlash());
        }

        if (CompareTag("Player") && UIManager.Instance != null)
        {
            UIManager.Instance.SetHP(currentHP);
        }

        if (currentHP <= 0)
        {
            currentHP = 0;
            isDead = true;
            //Player die
            if (CompareTag("Player"))
            {
                Debug.Log("GAME OVER");
                if (UIManager.Instance != null) 
                    UIManager.Instance.ShowGameOver();
                Time.timeScale = 0f;
            }
            else
            {
                // Enemy Die
                isDead = true;


                if (!CompareTag("SummonedEnemy"))
                {
                    WaveManager waveManager = FindFirstObjectByType<WaveManager>();
                    if (waveManager != null)
                    {
                        waveManager.EnemyKilled();
                    }
                }
                if (GetComponent<SummonerBossAI>() != null)
                {
                    if (BossHealthBarUI.Instance != null)
                    {
                        BossHealthBarUI.Instance.HideBossHealth();
                    }
                }
                if (anim != null) anim.SetTrigger("Dead");

                EnemyAI ai = GetComponent<EnemyAI>();
                if (ai != null) ai.enabled = false;

                Collider col = GetComponent<Collider>();
                if (col != null) col.enabled = false;

                CharacterController cc = GetComponent<CharacterController>();
                if (cc != null) cc.enabled = false;

                SummonerBossAI summoner = GetComponent<SummonerBossAI>();
                if (summoner != null)
                {
                    summoner.ClearSummonedEnemies();
                }
                Destroy(gameObject, 3f);
            }
        }
    }
    private System.Collections.IEnumerator HitFlash()
    {
        if (isFlashing) yield break;
        isFlashing = true;

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null && renderers[i].material.HasProperty("_Color"))
            {
                renderers[i].material.color = new Color(1f,0.3f,0.3f);
            }
        }

        yield return new WaitForSeconds(0.08f);

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null && renderers[i].material.HasProperty("_Color"))
            {
                renderers[i].material.color = originalColors[i];
            }
        }

        isFlashing = false;
    }
}
