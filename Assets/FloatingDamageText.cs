using TMPro;
using UnityEngine;

public class FloatingDamageText : MonoBehaviour
{
    public float floatSpeed = 60f;
    public float lifeTime = 0.8f;

    private TextMeshProUGUI text;
    private float timer;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void Setup(int damage, bool isCritical)
    {
        if (isCritical)
        {
            text.text = damage+"!";
            text.color = Color.red;
            text.fontSize = 30;
        }
        else
        {
            text.text = damage.ToString();
            text.color = Color.white;
            text.fontSize = 20;
        }
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}