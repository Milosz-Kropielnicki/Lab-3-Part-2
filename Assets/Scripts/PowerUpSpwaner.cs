using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject powerupPrefab;

    // Time between spawns in seconds
    private float respawnTime = 10.0f;

    // Far edge of the plane (where powerups appear)
    private float spawnXBound = 20.0f;
    private float spawnZ = 22.0f;   // Far side (near enemy formation)

    // Where the powerup is considered "gone" past the player
    private float despawnZ = -10.0f;

    private GameObject currentPowerup;
    private float timer;

    void Start()
    {
        SpawnPowerup();
    }

    void Update()
    {
        // Restart timer once the powerup is collected or has scrolled past
        if (currentPowerup == null)
        {
            timer -= Time.deltaTime;

            if (timer <= 0.0f)
            {
                SpawnPowerup();
            }
        }
    }

    void SpawnPowerup()
    {
        float x = Random.Range(-spawnXBound, spawnXBound);
        Vector3 spawnPosition = new Vector3(x, 0.0f, spawnZ);

        currentPowerup = Instantiate(powerupPrefab, spawnPosition, Quaternion.identity);

        // Tell the powerup where to stop so it can self-destroy if missed
        currentPowerup.GetComponent<PowerupController>().Init(Vector3.back, despawnZ);

        timer = respawnTime;
    }
}