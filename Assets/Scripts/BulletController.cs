using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float speed = 15.0f;

    // Destroy the bullet after this period
    private float lifetime = 5.0f;

    private Vector3 direction = Vector3.back;

    private bool firedByPlayer;

    public void Init(Vector3 moveDirection, bool isPlayerBullet)
    {
        direction = moveDirection;
        firedByPlayer = isPlayerBullet;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (firedByPlayer && other.TryGetComponent(out EnemyController enemy))
        {
            enemy.TakeDamage(1);
            Destroy(gameObject);
        }
        else if (!firedByPlayer && other.TryGetComponent(out PlayerController player))
        {
            player.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
