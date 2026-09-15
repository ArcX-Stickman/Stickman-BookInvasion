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
    public GameObject Stick;
    public Transform Targ;
    public float OriginalSize;
    private IEnumerator enumerator;

    void Start()
    {
        // Only allow ONE CameraSukugaCuts to exist.
        if (instance == null)
        {
            instance = this;
        }
        OriginalSize = virtualCamera.m_Lens.FieldOfView;
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

    public IEnumerator Zoom(float targetSize, bool reverse)
    {
        if (virtualCamera == null)
            yield break;

        StopCurrentZoom();

        enumerator = ZoomInternal(targetSize, Speed);
        yield return StartCoroutine(enumerator);

        if (reverse)
        {
            enumerator = ZoomInternal(Size, Speed);
            yield return StartCoroutine(enumerator);
        }
       



        enumerator = null;
    }

    public IEnumerator Zoom(float targetSize, bool reverse, float gap)
    {
        if (virtualCamera == null)
            yield break;

        StopCurrentZoom();

        enumerator = ZoomInternal(targetSize, Speed);
        yield return StartCoroutine(enumerator);

        if (reverse)
        {
            yield return new WaitForSecondsRealtime(gap);

            enumerator = ZoomInternal(Size, Speed);
            yield return StartCoroutine(enumerator);
        }
      



        enumerator = null;
    }


    public IEnumerator Zoom(float targetSize, bool reverse, float gap, CinemachineImpulseSource impulseSource, float impulseVel)
    {
        if (virtualCamera == null)
            yield break;

        StopCurrentZoom();

        enumerator = ZoomInternal(targetSize, Speed);
        yield return StartCoroutine(enumerator);

        if (reverse)
        {
            if (impulseSource != null)
            {
                impulseSource.GenerateImpulse(impulseVel);
            }
            yield return new WaitForSecondsRealtime(gap);

            enumerator = ZoomInternal(Size, Speed);
            yield return StartCoroutine(enumerator);
        }
        else
        {
            if (impulseSource != null)
            {
                impulseSource.GenerateImpulse(impulseVel);
            }
        }

      


        enumerator = null;
    }


    public IEnumerator Zoom(float targetSize, bool reverse, float gap, Transform Targ2)
    {
        if (virtualCamera == null)
            yield break;

        StopCurrentZoom();

        enumerator = ZoomInternal(targetSize, Speed);
        yield return StartCoroutine(enumerator);

        if (reverse)
        {
            yield return new WaitForSecondsRealtime(gap);

            enumerator = ZoomInternal(Size, Speed);
            Targ = Targ2;
            yield return StartCoroutine(enumerator);
            Targ = OGTarg;
        }
       


        enumerator = null;
    }

    public IEnumerator Zoom(float targetSize, bool reverse, float gap, Transform Targ2, bool beforeTargSwitch)
    {
        if (virtualCamera == null)
            yield break;

        StopCurrentZoom();
        Targ = Targ2;

        enumerator = ZoomInternal(targetSize, Speed);
        yield return StartCoroutine(enumerator);

        if (reverse)
        {
            yield return new WaitForSecondsRealtime(gap);

            enumerator = ZoomInternal(Size, Speed);
            yield return StartCoroutine(enumerator);
            Targ = OGTarg;

        }



        enumerator = null;
    }


    public IEnumerator Zoom(float targetSize, float speed)
    {
        if (virtualCamera == null)
        {
            Debug.LogError(" yo bro cam was null i aint doin this");
            yield break;

        }

        StopCurrentZoom();

        enumerator = ZoomInternal(targetSize, speed);
        yield return StartCoroutine(enumerator);
        Debug.Log(" yo bro i did sum");

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

    public IEnumerator ZoomCamIn(float Max, float MinusThing)
    {
       
        // minus is speed btw

        CinemachineVirtualCamera CVM = GetComponent<CinemachineVirtualCamera>();

        while (CVM.m_Lens.FieldOfView > Max)
        {
            CVM.m_Lens.FieldOfView -= MinusThing * Time.unscaledDeltaTime;
            yield return null;
        }

        yield break;

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
            virtualCamera.m_Lens.FieldOfView = Mathf.MoveTowards(
                virtualCamera.m_Lens.FieldOfView,
                targetSize,
                speed * Time.unscaledDeltaTime
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

    private bool SpeicalShake;
    public IEnumerator ShakeCamDirection(float intensity, float time, Transform reference, bool Flip)
    {
        if (SpeicalShake) yield break;
        SpeicalShake = true;

        var vcam = virtualCamera;
        var transposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();

        Vector3 originalOffset = transposer.m_TrackedObjectOffset;

        Transform target = Stick.transform;
        float dirSign = reference.localScale.x >= 0 ? 1f : -1f;

        Vector3 worldDir = reference.right * dirSign;
        worldDir.y = 0f;
        worldDir.Normalize();
        worldDir.y = 0f;

        float elapsed = 0f;

        if (Flip)
        {
            worldDir = reference.right * -dirSign;
        }

        while (elapsed < time)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = elapsed / time;

            float strength = intensity * (1f - t * t);

            transposer.m_TrackedObjectOffset = originalOffset + worldDir * (strength * 1.3f);

            yield return null;
        }

        elapsed = 0f;


        float timeOut = 0.2f;
        float timer4Out = 0;

  
        float returnTime = 0f;

        while (returnTime < 0.15f)
        {
            returnTime += Time.unscaledDeltaTime;

            transposer.m_TrackedObjectOffset = Vector3.Lerp(
                transposer.m_TrackedObjectOffset,
                originalOffset,
                returnTime / 0.15f
            );

            yield return null;
        }

        transposer.m_TrackedObjectOffset = originalOffset;
        SpeicalShake = false;
    }
    public Transform OGTarg;
    private void Update()
    {
        if (OGTarg == null) return;
        if (Targ != null)
        {
            virtualCamera.m_Follow = Targ.transform;
        }
        else
        {
            virtualCamera.m_Follow = OGTarg.transform;

        }
    }

    public IEnumerator SwitchTargets(Transform pos)
    {
        Debug.Log("Yess We Switched targets too  " + pos.name);
        Targ = pos;
        yield break;
    }

    public IEnumerator SwitchTargets(Transform pos, float time)
    {
        Targ = pos;
        yield return new WaitForSecondsRealtime(time);
        Targ = OGTarg;



        yield break;
    }

}