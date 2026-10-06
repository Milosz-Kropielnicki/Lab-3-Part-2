using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private Rigidbody rb;

    private float speed = 5.0f;

    private float xBound = 32.0f;

    private NavMeshAgent navMeshAgent;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector3(speed, 0.0f, 0.0f);

        Vector3 position = rb.position;
        position.x = Mathf.Clamp(position.x, -xBound, xBound);
        rb.position = position;

        if (CheckBounds(position))
        {
            speed *= -1.0f;
        }
    }

    bool CheckBounds(Vector3 position)
    {
        if (position.x == xBound)
        {
            return true;
        }

        return false;
    }
}


