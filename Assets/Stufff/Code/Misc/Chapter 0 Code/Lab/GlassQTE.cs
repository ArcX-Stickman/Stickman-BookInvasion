using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlassQTE : MonoBehaviour
{

    public GameObject BrokenGlass;
    public Movement Stickman;
    public GameObject ParticlesGlass;
    public CameraScript CameraScript;
    public Cinemachine.CinemachineVirtualCamera StickCam; // <--- Thats the default cam
    public Cinemachine.CinemachineVirtualCamera QTE_Cam;

    // make cam size --> 0.555 screen y 
    void OnEnable()
    {
        BrokenGlass.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("ObjectOfDestruction"))
        {
            StickCam.Priority = -67;
            QTE_Cam.Priority = 0;
            QTE_Cam.enabled = true;
            Stickman.TimeManager.SloMO();
            BrokenGlass.gameObject.SetActive(true);
            ParticlesGlass.gameObject.SetActive(true);
           // StartCoroutine(CameraScript.SmoothZoomIn(35.3f, 1));
            Destroy(gameObject);
           
        }
    }
}
