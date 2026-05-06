using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class HeavyBossAI : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip dashWarningClip;
    public AudioClip dashClip;
    public AudioClip attackClip;
    public AudioClip runClip;
    public AudioClip deathClip;

    [Header("Attack Settings")]
    public float attackRange = 2.5f;
    public int attackDamage = 30;
    public float attackCooldown = 2f;

    [Header("Dash Settings")]
    public float dashCooldown = 6f;
    public float dashWarningTime = 1f;
    public float dashSpeed = 12f;
    public float dashDuration = 0.4f;
    public int dashDamage = 50;
    public GameObject dashWarningPrefab;
    public float warningLength = 10f;
    public float warningWidth = 2f;
    public float dashHitRange = 1.5f;

    private Transform player;
    private Health health;
    private Animator anim;
    private NavMeshAgent agent;

    private float attackTimer;
    private float dashTimer;
    private bool isDashing = false;
    private bool isDead = false;
    private bool isAttacking = false;
    private bool isPlayingRun = false;

    private const string KEY_SFX = "SETTINGS_SFX";

    private float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat(KEY_SFX, 1f);
    }

    void Start()
    {
        health = GetComponent<Health>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;
        }

        dashTimer = dashCooldown;
    }

    void Update()
    {
        if (player == null) return;

        if (health != null && health.currentHP <= 0)
        {
            if (!isDead)
            {
                isDead = true;

                // 停止跑步声
                if (audioSource != null)
                    audioSource.Stop();

                // ? 播放死亡音效
                if (audioSource != null && deathClip != null)
                    audioSource.PlayOneShot(deathClip, GetSFXVolume());

                if (anim != null)
                    anim.SetTrigger("Dead");
            }

            return;
        }

        if (isDead) return;

        // ??? 一直追玩家（核心！！）
        if (!isDashing && !isAttacking && agent != null)
        {
            agent.SetDestination(player.position);
            // ? 播放跑步声
            if (!isPlayingRun && runClip != null)
            {
                audioSource.clip = runClip;
                audioSource.loop = true;
                audioSource.Play();
                isPlayingRun = true;
            }
        }
        else
        {
            // ? 停止跑步声
            if (isPlayingRun)
            {
                audioSource.Stop();
                isPlayingRun = false;
            }
        }

        // ? 更新动画Speed
        if (anim != null && agent != null)
        {
            anim.SetFloat("Speed", agent.velocity.magnitude);
        }

        if (isDashing || isAttacking) return;

        HandleAttack();
        HandleDash();
    }

    void HandleAttack()
    {
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

            StartCoroutine(AttackRoutine());

            Debug.Log("Heavy Boss attacked player.");
        }
    }

    void HandleDash()
    {
        dashTimer -= Time.deltaTime;

        if (dashTimer <= 0f)
        {
            dashTimer = dashCooldown;
            StartCoroutine(DashAttack());
        }
    }

    IEnumerator DashAttack()
    {
        attackTimer = attackCooldown;
        isDashing = true;

        if (agent != null)
        {
            agent.isStopped = true;
        }

        if (anim != null)
        {
            anim.SetTrigger("Attack");
        }

        Debug.Log("Heavy Boss dash warning!");
        if (audioSource != null && dashWarningClip != null)
        {
            audioSource.PlayOneShot(dashWarningClip, GetSFXVolume());
        }

        Vector3 dashDir = player.position - transform.position;
        dashDir.y = 0f;
        dashDir = dashDir.normalized;

        GameObject warning = null;

        if (dashWarningPrefab != null)
        {
            float actualWarningLength = dashSpeed * dashDuration;

            Vector3 warningPos = transform.position + dashDir * (actualWarningLength / 2f);
            warningPos.y = 0.05f;

            warning = Instantiate(dashWarningPrefab, warningPos, Quaternion.LookRotation(dashDir));
            warning.transform.localScale = new Vector3(warningWidth, 0.05f, actualWarningLength);
        }

        yield return new WaitForSeconds(dashWarningTime);
        if (audioSource != null && dashClip != null)
        {
            audioSource.PlayOneShot(dashClip, GetSFXVolume());
        }

        if (warning != null)
        {
            Destroy(warning);
        }
        bool hitPlayer = false;
        float timer = 0f;
        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth == null)
        {
            yield break;
        }
  
        while (timer < dashDuration)
        {
            transform.position += dashDir * dashSpeed * Time.deltaTime;
            timer += Time.deltaTime;

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (!hitPlayer && distanceToPlayer <= dashHitRange)
            {
                playerHealth.TakeDamage(dashDamage);
                hitPlayer = true;
            }

            yield return null;
        }

        if (agent != null)
        {
            agent.Warp(transform.position);
            agent.isStopped = false;
        }

        isDashing = false;
    }
    IEnumerator AttackRoutine()
    {
        isAttacking = true;

        if (agent != null)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }

        if (anim != null)
        {
            anim.SetFloat("Speed", 0f);
            anim.SetTrigger("Attack");
            if (audioSource != null && attackClip != null)
            {
                audioSource.PlayOneShot(attackClip, GetSFXVolume());
            }
        }

        yield return new WaitForSeconds(0.8f);

        if (agent != null)
        {
            agent.isStopped = false;
        }

        isAttacking = false;
    }
}