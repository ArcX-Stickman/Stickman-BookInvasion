using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiologyBookObject : MonoBehaviour
{
    public Movement PlayerMovement;
    public Animator BioAnim;
    public bool CanThrow;

    // Start is called before the first frame update
    void Start()
    {
        PlayerMovement = FindObjectOfType<Movement>();
    }


    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collider is the Player
        if (collision.CompareTag("Player"))
        {
            BioAnim.SetTrigger("Throw");
            
        }
    }

    // This method calculates knockback and applies the force to the player
    public void KnockPlayer(Transform playerTransform)
    {
        // Calculate knockback direction
        Vector2 hitDirection = (playerTransform.position - transform.position).normalized;

        // Apply more horizontal knockback and less vertical knockback for an arc
        float upwardForce = 1f; // Reduced upward force
        float horizontalKnockbackForce = 7f; // Increased horizontal force

        // Call knockback on the player with reduced upward and stronger horizontal force
        PlayerMovement.CallKnockBack(new Vector2(hitDirection.x * horizontalKnockbackForce, upwardForce), Vector2.zero, 0f);
    }

    // This method can be called in an animation event or elsewhere
   
}
