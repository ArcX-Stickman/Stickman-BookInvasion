using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake_AnimationKey : MonoBehaviour
{

    private Movement Stick;
    public float ShakeAmount;
    public float Duration;

    public void Shake()
    {
        Stick = FindAnyObjectByType<Movement>();
        
        Stick.CameraScript.StartCoroutine(Stick.CameraScript.ShakeCamGroundFeeling(55.5f,0.4f));
    }

    public void ShakeWithGivenAmount()
    {
        Stick = FindAnyObjectByType<Movement>();

        Stick.CameraScript.StartCoroutine(Stick.CameraScript.ShakeCamGroundFeeling(ShakeAmount, Duration));
    }
}
