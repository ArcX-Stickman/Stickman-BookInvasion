using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour
{
    private void Update()
    {
        // Check if the parent has a Rigidbody2D and get its velocity
        Rigidbody2D rb = GetComponentInParent<Rigidbody2D>();
        if (rb != null)
        {
            // Flip the sprite based on the direction of movement
            if (rb.velocity.x > 0.01f)
            {
                // Facing right
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (rb.velocity.x < -0.01f)
            {
                // Facing left
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }
}
