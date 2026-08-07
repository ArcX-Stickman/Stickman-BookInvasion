using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSukugaCuts : MonoBehaviour
{
    public float Speed;
    public CinemachineVirtualCamera virtualCamera;
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

    public IEnumerator Zoom(float targetSize)
    {
        if (virtualCamera == null) yield break;

        while (Mathf.Abs(virtualCamera.m_Lens.FieldOfView - targetSize) > 0.01f)
        {
            virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(
                virtualCamera.m_Lens.FieldOfView,
                targetSize,
                Speed * Time.deltaTime
            );

            yield return null;
        }

        virtualCamera.m_Lens.FieldOfView = targetSize;
    }
}
