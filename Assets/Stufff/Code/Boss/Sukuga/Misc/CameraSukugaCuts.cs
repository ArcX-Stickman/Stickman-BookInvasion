using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSukugaCuts : MonoBehaviour
{

    public static CameraSukugaCuts instance;

    void Start()
    {
        if(instance == null)
        {
            instance = this;

        }
    }

    public void CameraShake(CinemachineImpulseSource source, float intensity)
    {
        source.GenerateImpulseWithForce(intensity);
    }
}
