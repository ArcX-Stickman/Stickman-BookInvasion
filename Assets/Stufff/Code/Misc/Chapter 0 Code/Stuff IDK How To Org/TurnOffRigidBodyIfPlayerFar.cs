using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffRigidBodyIfPlayerFar : MonoBehaviour
{
    private Rigidbody2D rb;
    public float Distance;
    public float MaxDistance;
    public Transform StickPOS;
    public bool WantToReturntoPool;
    public bool isOg;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Distance = Vector2.Distance(transform.position, StickPOS.position);

        if(Distance > MaxDistance)
        {
            rb.simulated = false;
            if(WantToReturntoPool && !isOg)
            {
                ObjectPooler.ReturnObjectToPool(gameObject);
            }
        }
        else
        {
            rb.simulated = true;
        }
    }
}
