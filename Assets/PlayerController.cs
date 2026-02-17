using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;

    public GameObject bulletPrefab;
    public float bulletSpeed = 14f;
    public float fireCooldown = 0.2f;

    private CharacterController cc;
    private Vector3 facingDir;
    private float fireTimer;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        facingDir = Vector3.forward;
    }

    void Update()
    {
        fireTimer -= Time.deltaTime;

        // WASD -> 世界坐标移动（上=+Z，右=+X）
        float x = 0f, z = 0f;
        if (Input.GetKey(KeyCode.A)) x -= 1f;
        if (Input.GetKey(KeyCode.D)) x += 1f;
        if (Input.GetKey(KeyCode.W)) z += 1f;
        if (Input.GetKey(KeyCode.S)) z -= 1f;

        Vector3 input = new Vector3(x, 0f, z);
        if (input.sqrMagnitude > 0.001f)
        {
            Vector3 dir = input.normalized;

            // 角色朝向跟随移动方向
            facingDir = dir;
            transform.rotation = Quaternion.LookRotation(facingDir, Vector3.up);

            cc.Move((dir * moveSpeed + Vector3.down * 2f) * Time.deltaTime);
        }
        else
        {
            // 不动也贴地
            cc.Move(Vector3.down * 2f * Time.deltaTime);
        }

        Shoot();
    }

    void Shoot()
    {
        if (!Input.GetKey(KeyCode.J)) return;
        if (fireTimer > 0f) return;
        if (bulletPrefab == null) return;

        fireTimer = fireCooldown;
        var b = Instantiate(bulletPrefab, transform.position + Vector3.up * 0.8f, Quaternion.identity);
        var rb = b.GetComponent<Rigidbody>();
        if (rb != null) rb.velocity = facingDir * bulletSpeed;
    }
}
