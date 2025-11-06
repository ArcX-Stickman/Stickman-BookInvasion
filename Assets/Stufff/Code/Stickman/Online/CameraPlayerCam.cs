using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CameraPlayerCam : NetworkBehaviour
{
    public Cinemachine.CinemachineVirtualCamera vCam;

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            return;
            

        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            vCam.Follow = player.transform;
        }
        else
        {
            Debug.LogWarning("yO WHERE IS HEE");
        }

    }
}
