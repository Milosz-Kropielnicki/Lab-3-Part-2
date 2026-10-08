using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float speed = 15.0f;

    // Destroy the bullet after this period
    private float lifetime = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.back * speed * Time.deltaTime;
    }
}
