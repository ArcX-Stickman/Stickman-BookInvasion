using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStickSukuga : MonoBehaviour
{

    public Transform FinalDes;
    public float MinDis;
    public StickCinematicVer_BossFight StickSUK;
    public float speed;


    private Rigidbody rb3D;


    private void Start()
    {
        rb3D = StickSUK.GetComponent<Rigidbody>();
    }


    void Update()
    {
        if (Distance() > MinDis)
        {
            // Disable physics while cinematic movement happens
            if (StickSUK.rb != null)
            {
                StickSUK.rb.simulated = false;
            }

            if (rb3D != null)
            {
                rb3D.isKinematic = true;
            }


            StickSUK.StickAnim.SetBool("Jump", true);


            StickSUK.transform.position = Vector3.MoveTowards(
                StickSUK.transform.position,
                FinalDes.position,
                speed * Time.deltaTime
            );


            StickSUK.enabled = false;
        }
        else
        {
            // Re-enable physics
            if (StickSUK.rb != null)
            {
                StickSUK.rb.simulated = true;
            }

            if (rb3D != null)
            {
                rb3D.isKinematic = false;
            }


            StickSUK.enabled = true;
            this.enabled = false;
        }
    }


    public float Distance()
    {
        return Vector3.Distance(
            StickSUK.transform.position,
            FinalDes.position
        );
    }
}