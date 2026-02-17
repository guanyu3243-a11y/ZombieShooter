using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public int damagePerTick = 10;
    public float tickInterval = 1f;

    private Transform player;
    private CharacterController cc;
    private float timer;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
        timer = 0f;
    }

    void Update()
    {
        if (player == null) return;

        // в╥вы
        Vector3 dir = player.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.25f)
        {
            Vector3 moveDir = dir.normalized;
            transform.rotation = Quaternion.LookRotation(moveDir, Vector3.up);
            cc.Move((moveDir * moveSpeed + Vector3.down * 2f) * Time.deltaTime);
        }

        timer -= Time.deltaTime;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!hit.collider.CompareTag("Player")) return;

        if (timer <= 0f)
        {
            timer = tickInterval;
            Health hp = hit.collider.GetComponent<Health>();
            if (hp != null) hp.TakeDamage(damagePerTick);
        }
    }
}
