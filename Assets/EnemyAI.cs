using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public int damagePerTick = 10;
    public float tickInterval = 1f;

    private Transform player;
    private NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        if (agent != null)
        {
            agent.speed = moveSpeed;
            agent.stoppingDistance = 1.2f;
        }

        timer = 0f;
    }

    void Update()
    {
        if (player == null || agent == null) return;

        agent.speed = moveSpeed;
        agent.SetDestination(player.position);

        timer -= Time.deltaTime;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= agent.stoppingDistance + 0.3f)
        {
            TryAttackPlayer();
        }
    }

    void TryAttackPlayer()
    {
        if (timer > 0f) return;

        timer = tickInterval;

        Health hp = player.GetComponent<Health>();
        if (hp != null)
        {
            hp.TakeDamage(damagePerTick);
        }
    }
}