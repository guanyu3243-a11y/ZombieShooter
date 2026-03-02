using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 1.5f;
    public int maxAlive = 10;
    public float spawnRadius = 14f;

    private Transform player;
    private float timer;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
        timer = 0f;
    }

    void Update()
    {
        if (enemyPrefab == null || player == null) return;

        timer -= Time.deltaTime;
        if (timer > 0f) return;

        int alive = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (alive >= maxAlive) return;

        timer = spawnInterval;
        //spawner position
        Vector2 r = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 pos = player.position + new Vector3(r.x, 0f, r.y);

        //Generate enemies
        Instantiate(enemyPrefab, pos, Quaternion.identity);
    }
}
