using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 20;
    public float lifeTime = 2f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        Health h = other.GetComponent<Health>();
        if (h != null)
        {
            int finalDamage = damage;

            if (PlayerStats.Instance != null)
            {
                finalDamage = Mathf.RoundToInt(damage * PlayerStats.Instance.bulletDamageMultiplier);
            }

            h.TakeDamage(finalDamage);
            Debug.Log("Bullet Damage: " + finalDamage);
        }

        Destroy(gameObject);
    }
}