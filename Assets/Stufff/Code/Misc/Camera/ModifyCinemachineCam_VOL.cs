using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyCinemachineCam_VOL : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineFramingTransposer framingTransposer;

    public float LookaheadTime;

    public void Start()
    {
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            framingTransposer.m_LookaheadTime = LookaheadTime;
        }
    }
}
