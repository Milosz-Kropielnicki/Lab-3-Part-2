using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float speed = 15.0f;

    // Destroy the bullet after this period
    private float lifetime = 5.0f;

    private Vector3 direction = Vector3.back;

    private bool firedByPlayer;
    private int damage = 1;

    public void Init(Vector3 moveDirection, bool isPlayerBullet)
    {
        Init(moveDirection, isPlayerBullet, 1);
    }

    public void Init(Vector3 moveDirection, bool isPlayerBullet, int bulletDamage)
    {
        direction = moveDirection;
        firedByPlayer = isPlayerBullet;
        damage = bulletDamage;
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (firedByPlayer && other.TryGetComponent(out EnemyController enemy))
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (!firedByPlayer && other.TryGetComponent(out PlayerController player))
        {
            player.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}