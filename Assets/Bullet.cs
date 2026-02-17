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
            h.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
