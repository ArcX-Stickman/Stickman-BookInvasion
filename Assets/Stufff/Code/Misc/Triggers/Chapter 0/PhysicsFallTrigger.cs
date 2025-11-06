using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsFallTrigger : MonoBehaviour
{

    public MoveTowardsPosition MTP;
    public GameObject Physics;
    private bool Once;
    void Update()
    {
        if(Physics == null && !Once)
        {
            Once = true;
            StartCoroutine(SpeedController());
        }
    }

    IEnumerator SpeedController()
    {
        MTP.SecondDisNow = true;
        yield return new WaitForSecondsRealtime(2);
        MTP.OriginalSpeed = 110;
        yield return new WaitForSecondsRealtime(1);
        MTP.OriginalSpeed = 160;
        yield return new WaitForSecondsRealtime(4);
        MTP.OriginalSpeed = 210;
        this.enabled = false;
        yield return null;
    }
}
