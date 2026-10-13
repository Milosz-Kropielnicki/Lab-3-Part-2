using UnityEngine;

public class EnemyFormation : MonoBehaviour
{
    private float speed = 5.0f;

    private float stepDown = 1.0f;

    private float xBound = 20.0f;

    private int direction = 1;

    // Distance from the formation pivot to the rightmost enemy (local X, positive).
    private float rightExtent;

    // Distance from the formation pivot to the leftmost enemy (local X, negative).
    private float leftExtent;

    void Start()
    {
        RecalculateExtents();
    }

    void RecalculateExtents()
    {
        float minX = 0f;
        float maxX = 0f;

        bool found = false;

        foreach (Transform child in transform)
        {
            // Only count actual enemies. Skip markers, empties, etc.
            if (!child.TryGetComponent(out EnemyController _))
            {
                continue;
            }

            float x = child.localPosition.x;

            if (!found)
            {
                minX = x;
                maxX = x;
                found = true;
            }
            else
            {
                if (x < minX) minX = x;
                if (x > maxX) maxX = x;
            }
        }

        rightExtent = maxX;
        leftExtent = minX;
    }

    void Update()
    {
        transform.position += Vector3.right * direction * speed * Time.deltaTime;

        float rootX = transform.position.x;

        bool hitRight = direction > 0 && rootX + rightExtent >= xBound;
        bool hitLeft = direction < 0 && rootX + leftExtent <= -xBound;

        if (hitRight || hitLeft)
        {
            float clampedRootX = hitRight
                ? xBound - rightExtent
                : -xBound - leftExtent;

            Vector3 pos = transform.position;
            pos.x = clampedRootX;
            pos.z -= stepDown;
            transform.position = pos;

            direction *= -1;
        }
    }
}