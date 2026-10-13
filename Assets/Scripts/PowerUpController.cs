using UnityEngine;

public class PowerupController : MonoBehaviour
{
    private float speed = 6.0f;

    private Vector3 direction = Vector3.back;

    private float despawnZ = -10.0f;

    // Spin for visibility
    private float spinSpeed = 90.0f;

    public void Init(Vector3 moveDirection, float despawnZPosition)
    {
        direction = moveDirection;
        despawnZ = despawnZPosition;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);

        // Missed by the player — remove so spawner can start its timer
        if (transform.position.z <= despawnZ)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            player.UpgradeWeapon();
            Destroy(gameObject);
        }
    }
}