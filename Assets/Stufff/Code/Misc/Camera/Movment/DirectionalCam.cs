using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalCam : MonoBehaviour
{

    public CameraScript StickCam;
    public bool Flip;
    public float HoriShake;

    private void Start()
    {
        if (HoriShake <= 0) 
        {
            HoriShake = 5.5f;
        }
    }
    public void CamHold()
    {
        StickCam.HoldNeeeded = true;
    }

    public void CamUnHold()
    {
        StickCam.HoldNeeeded = false;

    }

    public void DiretionCamShake()
    {
        CameraScript CS = StickCam;
        StartCoroutine(CS.ShakeCamDirection(HoriShake, 0.2f, transform,Flip));
    }
    public void DiretionCamShakeVert(float Upf)
    {
        // when using this if you want to go up make the Upf vari 1 - i know this is really weird but unity animation system is a bum for not allowig boools >:( 
        bool Up = false;

        if(Upf < 0)
        {
            Up = true;
        }

        StartCoroutine(StickCam.ShakeCamDirection_Vertical(30, 0.5f,Up, transform));
    }

    private bool HoldNNah;
    public IEnumerator HoldNUnHold(float time)
    {
        if(HoldNNah) yield break;
        HoldNNah = true;
        float timer = 0;
        CamHold();
        while (timer < time)
        {
            time += Time.deltaTime;
            yield return null;
        }
        StickCam.HoldNeeeded = false;
        CamUnHold();
        yield break;
    }
}
