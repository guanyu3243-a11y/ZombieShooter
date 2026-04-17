using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("Player Stats")]
    public float bulletDamageMultiplier = 1f;
    public float fireRateMultiplier = 1f;

    void Awake()
    {
        Instance = this;
    }

    public void IncreaseDamage(float amount)
    {
        bulletDamageMultiplier += amount;
        Debug.Log("Damage Multiplier: " + bulletDamageMultiplier);
    }

    public void IncreaseFireRate(float amount)
    {
        fireRateMultiplier += amount;
        Debug.Log("Fire Rate Multiplier: " + fireRateMultiplier);
    }

    public void HealPlayer(int healAmount)
    {
        Health hp = GetComponent<Health>();
        if (hp != null)
        {
            hp.currentHP = Mathf.Min(hp.currentHP + healAmount, hp.maxHP);

            if (UIManager.Instance != null)
            {
                UIManager.Instance.SetHP(hp.currentHP);
            }

            Debug.Log("Player healed to: " + hp.currentHP);
        }
    }
}
