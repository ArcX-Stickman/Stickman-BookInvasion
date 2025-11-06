using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCode : MonoBehaviour
{
    public Movement StickManager;
    public Health StickHP;
    public float PhysicsHP;
    public float speed;
    public Transform PlayerPOS;
    public Transform PhysicsPOS;
    public bool TouchingPlayer;
    public float MinDIS;
    public Animator PhysicsAnim;
    public Vision vision;
    public bool SpecialSitting;





    // Start is called before the first frame update
    void Start()
    {
        if (SpecialSitting)
        {
            PhysicsAnim.SetBool("Sitting", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (vision.PlayerSpotted == true)
        {
            PlayerSpotted();
            if (SpecialSitting)
            {
                PhysicsAnim.SetBool("Sitting", false);
            }
        }
    }


    public void PlayerSpotted()
    {
        if (StickHP.PlayerHP > 0)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, PlayerPOS.position);

            // Check if the player is within the minimum distance
            if (distanceToPlayer <= MinDIS)
            {
                TouchingPlayer = true;  // Set TouchingPlayer to true if within MinDIS
                PhysicsAnim.SetBool("Walk", false); // Stop walking animation
            }
            else
            {
                TouchingPlayer = false; // Set TouchingPlayer to false if outside MinDIS
                PhysicsAnim.SetBool("Walk", true); // Continue walking animation

                Vector2 targetPosition = new Vector2(PlayerPOS.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                // Flip the sprite based on direction of movement
                if (targetPosition.x < transform.position.x) // Moving left
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else if (targetPosition.x > transform.position.x) // Moving right
                {
                    transform.localScale = new Vector3(-1, 1, 1); // Face right
                }
            }

            // Trigger earthquake if touching player
            if (TouchingPlayer)
            {
                StickHP.InCombat = true;

                Debug.Log("PhysicsTouch");
            }
        }
    }
}
