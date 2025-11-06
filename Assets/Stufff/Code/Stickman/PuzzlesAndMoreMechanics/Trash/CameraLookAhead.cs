using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BROOO I FOLLOWED A TUTORIAL FOR A FLIPPING SPACESHIP 20 minutes that im not gettting back :(
/// </summary>
public class CameraLookAhead : MonoBehaviour
{
    public Movement StickBOI;

    public float distanceMultiplier = 2;

    public float Responsiveness = 0.1f;

    Vector3 currestOffset;

    private void LateUpdate()
    {
        Vector3 targetPos = StickBOI.transform.position;

        targetPos.z = transform.position.z;
        Vector3 targetOffset =  StickBOI.transform.right * StickBOI.MaxSpeed * distanceMultiplier;
        currestOffset = Vector3.Lerp(currestOffset,targetOffset, Responsiveness * Time.deltaTime);
        targetPos += currestOffset;
        transform.position = targetPos;
    }
}
