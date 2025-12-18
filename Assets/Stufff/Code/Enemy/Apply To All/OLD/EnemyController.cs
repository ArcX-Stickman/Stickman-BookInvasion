using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float stepHeight = 0.5f;   // Height the enemy can step over
    public float stepSpeed = 2f;      // Speed for the step-up transition
    public float detectionDistance = 0.1f; // Distance to detect obstacles

    public Transform stepCheckLow;    // Attach the StepCheckLow object here
    public Transform stepCheckHigh;   // Attach the StepCheckHigh object here

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        CheckAndStep();
    }

    private void CheckAndStep()
    {
        // Raycast to detect an obstacle at low position and no obstacle at high position
        RaycastHit2D hitLow = Physics2D.Raycast(stepCheckLow.position, Vector2.right * transform.localScale.x, detectionDistance);
        RaycastHit2D hitHigh = Physics2D.Raycast(stepCheckHigh.position, Vector2.right * transform.localScale.x, detectionDistance);

        // If an obstacle is detected low but not high, move the enemy up
        if (hitLow.collider != null && hitHigh.collider == null)
        {
            StepUp();
        }
    }

    private void StepUp()
    {
        Vector2 newPosition = rb.position + new Vector2(0, stepHeight);
        rb.MovePosition(Vector2.Lerp(rb.position, newPosition, stepSpeed * Time.deltaTime));
    }
}
