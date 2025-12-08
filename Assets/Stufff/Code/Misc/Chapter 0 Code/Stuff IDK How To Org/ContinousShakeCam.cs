using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinousShakeCam : MonoBehaviour
{
    public static ContinousShakeCam Instance;
    public float shakeforce;
    public CinemachineImpulseSource ImpulseSource;

    public float ShakeCD;
    private float LastShake = 0;

    // Start is called before the first frame update

    // Helicopter Chapter 0 - Notes
    // Glass QTE Casm size: 35.3

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(Time.time >= ShakeCD + LastShake)
        {
            LastShake = Time.time;
            Instance.ShakeDatCam(ImpulseSource);

        }
    }

    public void ShakeDatCam(CinemachineImpulseSource inpulseSource)
    {
        inpulseSource.GenerateImpulseWithForce(shakeforce);
    }


    //public IEnumerator ShakeCam(float intensity, float time)
    //{
    //    if (notlocked) yield break;
    //    if (inShake) yield break;
    //    if (!inShake)
    //    {
    //        inShake = true;
    //        tempCamSize = CamSize;
    //        float tempCalc = CamSize - intensity;
    //        while (CamSize != tempCalc)
    //        {
    //            CamSize = Mathf.Lerp(CamSize, tempCalc, intensity);
    //            yield return null;
    //        }
    //        yield return new WaitForSeconds(time);
    //        while (CamSize != tempCamSize)
    //        {
    //            CamSize = Mathf.Lerp(CamSize, tempCamSize, intensity);
    //            yield return null;
    //        }
    //        CamSize = tempCamSize;
    //        inShake = false;
    //    }
    //}
}
