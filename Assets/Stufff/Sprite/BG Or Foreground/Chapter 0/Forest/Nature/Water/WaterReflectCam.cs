using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterReflectCam : MonoBehaviour
{
    public Transform player;
    public float waterY;

    void LateUpdate()
    {
        Vector3 p = player.position;
        float mirroredY = waterY - (p.y - waterY);

        transform.position = new Vector3(p.x, mirroredY, transform.position.z);
    }

}
