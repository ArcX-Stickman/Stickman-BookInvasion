using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ChangeCamSize : MonoBehaviour
{
    [Header("Bevahvior 1 - its jus if Stickman is in Volume the cam stays in that size til he out")]
    public bool Behavior1;



    public CameraScript CamScript;

    private float TempCam;


    private bool Stick_HAS_ENTERED_ATLEAST_ONCE;


    [Header("Parameter")]

    public float CamSizeZoomedin;
    public float CamSizeZoomDuration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            Stick_HAS_ENTERED_ATLEAST_ONCE = true;
            CamSwitcharoo();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            ManAintIn();
        }
    }



    public void CamSwitcharoo()
    {
        if (Behavior1)
        {
            TempCam = CamScript.OG_CAM_SIZE;
            CamScript.ZoomTo(CamSizeZoomedin, CamSizeZoomDuration);
        }
    }


    public void ManAintIn()
    {
        if (Stick_HAS_ENTERED_ATLEAST_ONCE)
        {
            if (Behavior1)
            {

                CamScript.ZoomTo(TempCam, CamSizeZoomDuration);

            }
        }
    }

       

    public void Behaviour1_END()
    {
        Stick_HAS_ENTERED_ATLEAST_ONCE = false;
    }



}
