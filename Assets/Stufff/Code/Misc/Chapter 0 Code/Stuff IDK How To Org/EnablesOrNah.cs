using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablesOrNah : MonoBehaviour
{
    public GameObject objectUWant;
    public Transform player;
    public float maxDistance;
    public float Distance;
    public Animator animator;

    private void Start()
    {
        animator = objectUWant.GetComponent<Animator>();
    }
    void Update()
    {

        if(objectUWant == null)
        {
            objectUWant = gameObject;
        }


        //if (player == null || objectUWant == null)
        //    return;

        float distance = Vector2.Distance(transform.position, player.position);
        Distance = distance;
        if (distance <= maxDistance)
        {
            objectUWant.SetActive(true);
            if (animator != null)
            {
                animator.enabled = true;
            }
        }
        else
        {
            objectUWant.SetActive(false);
            if(animator != null)
            {
                animator.enabled = false;
            }
        }
    }
}
