using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSukugaCuts : MonoBehaviour
{
    public float Size;
    public float Speed;
    public float Speed2;

    public CinemachineVirtualCamera virtualCamera;
    public static CameraSukugaCuts instance;

    private IEnumerator enumerator;

    void Start()
    {
        // Only allow ONE CameraSukugaCuts to exist.
        if (instance == null)
        {
            instance = this;
        }
        
    }

    public void CameraShake(CinemachineImpulseSource source, float intensity)
    {
        if (source == null)
            return;

        source.GenerateImpulseWithForce(intensity);
    }

    public IEnumerator Zoom()
    {
        if (virtualCamera == null)
            yield break;

        StopCurrentZoom();

        enumerator = ZoomInternal(Size, Speed);
        yield return StartCoroutine(enumerator);

        enumerator = null;
    }

    public IEnumerator Zoom(float targetSize)
    {
        if (virtualCamera == null)
            yield break;

        StopCurrentZoom();

        enumerator = ZoomInternal(targetSize, Speed);
        yield return StartCoroutine(enumerator);

        enumerator = null;
    }

    public IEnumerator Zoom(float targetSize, float speed)
    {
        if (virtualCamera == null)
            yield break;

        StopCurrentZoom();

        enumerator = ZoomInternal(targetSize, speed);
        yield return StartCoroutine(enumerator);

        enumerator = null;
    }

    public IEnumerator ZoomAnim(float targetSize)
    {
        if (virtualCamera == null)
            yield break;

        StopCurrentZoom();

        enumerator = ZoomInternal(targetSize, Speed);
        yield return StartCoroutine(enumerator);

        enumerator = null;
    }

    public IEnumerator ZoomAnim2(float targetSize)
    {
        if (virtualCamera == null)
            yield break;

        StopCurrentZoom();

        enumerator = ZoomInternal(targetSize, Speed2);
        yield return StartCoroutine(enumerator);

        enumerator = null;
    }

    private IEnumerator ZoomInternal(float targetSize, float speed)
    {
        while (Mathf.Abs(virtualCamera.m_Lens.FieldOfView - targetSize) > 0.01f)
        {
            virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(
                virtualCamera.m_Lens.FieldOfView,
                targetSize,
                speed * Time.deltaTime
            );

            yield return null;
        }

        virtualCamera.m_Lens.FieldOfView = targetSize;
    }

    private void StopCurrentZoom()
    {
        if (enumerator != null)
        {
            StopCoroutine(enumerator);
            enumerator = null;
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}