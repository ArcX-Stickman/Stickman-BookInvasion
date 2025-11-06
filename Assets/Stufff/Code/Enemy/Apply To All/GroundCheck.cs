using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private Transform enemyFeet; // Position to adjust
    [SerializeField] private float stepHeight = 0.5f; // Height to step up
    [SerializeField] private string stepUpTag = "StepUp"; // Tag for objects to step up on

    private Rigidbody2D rb;
    private bool canStepUp = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Check if the enemy can step up
        if (canStepUp)
        {
            StepUp();
            canStepUp = false; // Reset after stepping up
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object has the specified tag
        if (collision.gameObject.CompareTag(stepUpTag))
        {
            Debug.Log("trying");
            canStepUp = true; // Enable stepping up
        }
    }

    private void StepUp()
    {
        // Calculate the new position to step up
        Vector2 newPosition = rb.position + new Vector2(0, stepHeight);
        rb.MovePosition(newPosition); // Move enemy up to step over the obstacle
    }

    private void OnDrawGizmos()
    {
        // Optional: Draw the position for visualization
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(enemyFeet.position, new Vector3(0.5f, 0.5f, 0)); // Adjust size as needed
    }
}
