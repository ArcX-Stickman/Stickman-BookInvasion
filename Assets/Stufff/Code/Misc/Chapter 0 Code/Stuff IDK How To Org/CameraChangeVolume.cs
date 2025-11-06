using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChangeVolume : MonoBehaviour
{
    public bool RevertingCam;
    public bool PlatformSectionPriorEcology;
    public CameraScript CameraScript;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HandleStuff();
        }
    }

    public void HandleStuff()
    {
        if (RevertingCam)
        {
            CameraScript.CamSize = CameraScript.OG_CAM_SIZE;
        }
        if (PlatformSectionPriorEcology)
        {
            CameraScript.CamSize = CameraScript.PlatformingBeforeceEcologySize;
        }
    }
}
