using UnityEngine;
using UnityEngine.AI;

public class EnemyFormation : MonoBehaviour
{
    private float speed = 5.0f;

    private float stepDown = 1.0f;

    private float xBound = 28.0f;

    private int direction = 1;

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * direction * speed * Time.deltaTime;

        if (HitBounds())
        {
            direction *= -1;

            transform.position += Vector3.back * stepDown;
        }
    }

    bool HitBounds()
    {
        foreach (Transform enemy in transform)
        {
            if (direction > 0 && enemy.position.x >= xBound)
            {
                return true;
            }
            else if (direction < 0 && enemy.position.x <= -xBound)
            {
                return true;
            }
        }

        return false;
    }
}


