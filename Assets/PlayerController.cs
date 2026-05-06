using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Dash")]
    public float dashDistance = 5f;
    public float dashCooldown = 5f;
    private float dashTimer = 0f;
    public float DashTimer => dashTimer; 
    public float DashCooldown => dashCooldown;
    

    public float moveSpeed = 6f;

    public GameObject bulletPrefab;
    public float bulletSpeed = 14f;
    public float fireCooldown = 0.5f;

    private CharacterController cc;
    private Vector3 facingDir;
    private float fireTimer;
    public AudioSource audioSource;
    public AudioClip shootClip;
    public AudioClip slowClip;
    private const string KEY_SFX = "SETTINGS_SFX";

    private float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat(KEY_SFX, 1f);
    }
    void Start()
    {
        cc = GetComponent<CharacterController>();
        facingDir = Vector3.forward;
        //moveSpeed = PlayerPrefs.GetFloat("SETTINGS_SENSITIVITY", moveSpeed);
    }

    void Update()
    {
        fireTimer -= Time.deltaTime;
        if (dashTimer > 0f)
        if (dashTimer > 0f)
        {
            dashTimer -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (PlayerStats.Instance != null)
            {
                PlayerStats.Instance.TryUseEnemySlow();
            }
        }
        HandleDash();
        // WASD -> movement
        float x = 0f, z = 0f;
        if (Input.GetKey(KeyCode.A)) x -= 1f;
        if (Input.GetKey(KeyCode.D)) x += 1f;
        if (Input.GetKey(KeyCode.W)) z += 1f;
        if (Input.GetKey(KeyCode.S)) z -= 1f;

        Vector3 input = new Vector3(x, 0f, z);
        if (input.sqrMagnitude > 0.001f)
        {
            Vector3 dir = input.normalized;//standardlizaing direction

            // the player's orientation follows the direction of movement
            facingDir = dir;
            transform.rotation = Quaternion.LookRotation(facingDir, Vector3.up);

            float finalMoveSpeed = moveSpeed;
            if (PlayerStats.Instance != null)
            {
                finalMoveSpeed = moveSpeed * PlayerStats.Instance.moveSpeedMultiplier;
            }

            cc.Move((dir * finalMoveSpeed + Vector3.down * 2f) * Time.deltaTime);
        }
        else
        {
            // follow the gravity
            cc.Move(Vector3.down * 2f * Time.deltaTime);
        }

        Shoot();
    }

    void Shoot()
    {
        if (!Input.GetKey(KeyCode.J)) return;
        if (fireTimer > 0f) return;
        if (bulletPrefab == null) return;

        float finalCooldown = fireCooldown;

        if (PlayerStats.Instance != null)
        {
            finalCooldown = fireCooldown / PlayerStats.Instance.fireRateMultiplier;
        }

        fireTimer = finalCooldown;
        if (audioSource != null && shootClip != null)
        {
            audioSource.PlayOneShot(shootClip,GetSFXVolume());
        }
        if (PlayerStats.Instance != null && PlayerStats.Instance.multiShotUnlocked)
        {
            ShootBullet(facingDir);
            ShootBullet(Quaternion.Euler(0, -15, 0) * facingDir);
            ShootBullet(Quaternion.Euler(0, 15, 0) * facingDir);
        }
        else
        {
            ShootBullet(facingDir);
        }
    }
    void ShootBullet(Vector3 direction)
    {
        var b = Instantiate(bulletPrefab, transform.position + Vector3.up * 0.8f, Quaternion.identity);
        var rb = b.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = direction.normalized * bulletSpeed;
        }
    }
    void HandleDash()
    {
        if (PlayerStats.Instance == null) return;
        if (!PlayerStats.Instance.dashUnlocked) return;
        if (dashTimer > 0f) return;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Vector3 dashDir = facingDir;

            if (dashDir.sqrMagnitude < 0.01f)
            {
                dashDir = transform.forward;
            }

            cc.Move(dashDir.normalized * dashDistance);
            dashTimer = dashCooldown;

            Debug.Log("Dash!");
        }
    }
}
