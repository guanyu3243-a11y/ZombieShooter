using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;
    private Animator anim;
    private bool isDead = false;


    void Awake()
    {
        currentHP = maxHP;
        anim = GetComponent<Animator>();

        if (CompareTag("Player") && UIManager.Instance != null)
        {
            UIManager.Instance.SetHP(currentHP);
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        currentHP -= amount;
        if (!CompareTag("Player") && anim != null)
        {
            anim.SetTrigger("Hit");
        }


        if (CompareTag("Player") && UIManager.Instance != null)
        {
            UIManager.Instance.SetHP(currentHP);
        }

        if (currentHP <= 0)
        {
            //Player die
            if (CompareTag("Player"))
            {
                Debug.Log("GAME OVER");
                if (UIManager.Instance != null) UIManager.Instance.ShowGameOver();
                Time.timeScale = 0f;
            }
            else
            {
                // Enemy Die,Score system
                isDead = true;

                if (UIManager.Instance != null) UIManager.Instance.AddScore(10);

                if (anim != null) anim.SetTrigger("Dead");

                EnemyAI ai = GetComponent<EnemyAI>();
                if (ai != null) ai.enabled = false;

                Collider col = GetComponent<Collider>();
                if (col != null) col.enabled = false;

                CharacterController cc = GetComponent<CharacterController>();
                if (cc != null) cc.enabled = false;

                Destroy(gameObject, 3f);
            }
        }
    }
}
