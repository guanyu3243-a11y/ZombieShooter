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
            bool isCritical = false;

            if (PlayerStats.Instance != null)
            {
                finalDamage = Mathf.RoundToInt(damage * PlayerStats.Instance.bulletDamageMultiplier);

                // ? ±©»÷ÅÐ¶Ï
                if (Random.value < PlayerStats.Instance.critChance)
                {
                    finalDamage = Mathf.RoundToInt(finalDamage * PlayerStats.Instance.critMultiplier);
                    isCritical = true;
                }
            }

            h.TakeDamage(finalDamage);
            

            // ? ÌáÊ¾
            if (isCritical)
            {
                Debug.Log("CRITICAL HIT! Damage: " + finalDamage);

            }
            else
            {
                Debug.Log("Bullet Damage: " + finalDamage);
            }
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowDamageText(
                    finalDamage,
                    isCritical,
                    other.transform.position + Vector3.up * 2f
                );
            }
        }

        Destroy(gameObject);
    }
}