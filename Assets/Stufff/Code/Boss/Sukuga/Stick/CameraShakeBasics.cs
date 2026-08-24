using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraShakeBasics : MonoBehaviour
{
    public CameraSukugaCuts Cam;
    public CinemachineImpulseSource Jump;
    public CinemachineImpulseSource Land;
    public CinemachineImpulseSource Rumble;
    private CinemachineBasicMultiChannelPerlin perlinNoise;

    void Start()
    {
        perlinNoise = Cam.virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    bool IsCameraShaking()
    {
        if (perlinNoise == null) return false;

        return perlinNoise.m_AmplitudeGain > 0f || perlinNoise.m_FrequencyGain > 0f;
    }

    public void JumpShake(float sha)
    {
       
        if (Cam == null || Jump == null)
        {
            return;
        }

        Cam.CameraShake(Jump, sha);
    }

    public void RumbleShake(float sha)
    {

        if (IsCameraShaking()) return;


        if (Cam == null || Land == null)
        {
            return;
        }

        Cam.CameraShake(Rumble, sha);
    }
    public void LandShake(float sha)
    {

        if (Cam == null || Land == null)
        {
            return;
        }

        Cam.CameraShake(Land, sha);
    }
}
