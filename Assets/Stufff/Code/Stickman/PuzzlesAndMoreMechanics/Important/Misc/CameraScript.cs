using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraScript : MonoBehaviour
{
    public CinemachineVirtualCamera StickCam;
    public float CamSize;
    public float shakeTime;
    private float IntensityStart;
    public float ShakeTimerTotal;
    private float tempCamSize;
    private bool inShake;
    public Movement StickManager;
    public CinemachineFramingTransposer transposer;
    private float AdditivePerspec = 90.6f;
    public float SizeForDeath;
    public float PlatformingBeforeceEcologySize; //102.2
    public float OG_CAM_SIZE;
    public GameObject Psych_Cutscene;
    public Camera MainCam;
    public bool notlocked;

    public Transform Stick;
    // Start is called before the first frame update

    // Helicopter Chapter 0 - Notes
    // Glass QTE Casm size: 35.3
    void Start()
    {
        StickCam = GetComponent<CinemachineVirtualCamera>();
        OG_CAM_SIZE = CamSize;
        transposer = StickCam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private float elapsedTime;
    public float durationofDeath = 4;
    private bool ALr_StartedDeath;
    private bool isDoingSomething;



    private void OnEnable()
    {
        if(StickManager.StickHP.PlayerHP > 0)
        {
            StopAllCoroutines();

        }
    }
    void Update()
    {
       
        if(StickManager.StickHP.PlayerHP > 0)
        {
            StickCam.m_Lens.OrthographicSize = CamSize;
            StickCam.m_Lens.FieldOfView = CamSize + AdditivePerspec;

        }
        else if(StickManager.StickHP.PlayerHP <= 0 &&!ALr_StartedDeath)
        {
            StickCam.m_Lens.OrthographicSize = CamSize;
            StickCam.m_Lens.FieldOfView = CamSize;
            ALr_StartedDeath = true;
            StartCoroutine(DeathCam());

        }
        if(Psych_Cutscene != null)
        {
            if (Psych_Cutscene.gameObject.activeSelf)
            {
                StickCam.Follow = Psych_Cutscene.transform;
                transposer.m_LookaheadSmoothing = 0;
                transposer.m_LookaheadTime = 0;
                transposer.m_YDamping = 0;
                transposer.m_ScreenY = 0.5f;
                StickManager.StopFootsteps();
                StickManager.gameObject.SetActive(false);

            }
            if (!Psych_Cutscene.activeSelf)
            {
                StickCam.Follow = Stick.transform;

            }
        }
        


    }
    public bool FullyZoomed;
    public IEnumerator DeathCam()
    {
        MainCam.orthographic = true;
        while (CamSize >= SizeForDeath && elapsedTime <= durationofDeath)
        {
            elapsedTime += Time.deltaTime;
            CamSize = Mathf.Lerp(StickCam.m_Lens.OrthographicSize, SizeForDeath, elapsedTime / durationofDeath);


            StickCam.m_Lens.OrthographicSize = CamSize;
            yield return null;
        }
        FullyZoomed = true;
    }


    public IEnumerator ZoomCam(float Size,float Duration)
    {
        MainCam.orthographic = true;
        while (CamSize >= Size && elapsedTime <= Duration)
        {
            elapsedTime += Time.deltaTime;
            CamSize = Mathf.Lerp(StickCam.m_Lens.OrthographicSize, Size, elapsedTime / Duration);


            StickCam.m_Lens.OrthographicSize = CamSize;
            yield return null;
        }


        elapsedTime = 0f;


        //FullyZoomed = true;
    }


    public IEnumerator ZoomOutCam(float Size, float Duration)
    {
        MainCam.orthographic = true;
        while (CamSize <= Size && elapsedTime <= Duration)
        {
            elapsedTime += Time.deltaTime;
            CamSize = Mathf.Lerp(StickCam.m_Lens.OrthographicSize, Size, elapsedTime / Duration);


            StickCam.m_Lens.OrthographicSize = CamSize;
            yield return null;
        }

        elapsedTime = 0f;


        //FullyZoomed = true;
    }

    public IEnumerator StickBrainWakeUp_Event()
    {
        //extend to 3492.2
        while(CamSize < 3492)
        {
            CamSize += 700 * Time.deltaTime;
            yield return null;
        }
    }

    private float PriorCamSizeToZoom;
    private bool inZoomCour;
    private bool inZoomOutCour;
    public IEnumerator ZoomIn(float Size, float time)
    {
        if(inZoomCour) yield break;
        PriorCamSizeToZoom = CamSize;
        inZoomCour = true;
        while(CamSize != Size)
        {
            CamSize = Mathf.Lerp (CamSize, Size, time);
            yield return null;
        }
        inZoomCour = false;
        yield return null;

    }

    public IEnumerator SmoothZoomIn(float targetSize, int duration)
    {
        float startCam = CamSize;
        float timer = 0;
        CamSize = Mathf.Lerp(CamSize, targetSize, duration);

        //while (timer <= duration)
        //{
        //    timer += 1 * Time.deltaTime;
        //    CamSize = Mathf.Lerp(CamSize,targetSize, duration);
        //    yield return null;
        //}
        yield return null;
    }


    public IEnumerator ZoomOut( float time) 
    {
        if (inZoomOutCour) yield break;
        inZoomOutCour = true;
        while(CamSize != PriorCamSizeToZoom) 
        {
            CamSize = Mathf.Lerp(CamSize, PriorCamSizeToZoom, time);
            yield return null;

        }
        inZoomOutCour = false;
        yield return null;
    }


    public IEnumerator ShakeCam(float intensity, float time)
    {
        if(notlocked) yield break; 
        if (inShake) yield break;
        if (!inShake)
        {
            inShake = true;
            tempCamSize = CamSize;
            float tempCalc = CamSize - intensity;
            while (CamSize != tempCalc)
            {
                CamSize = Mathf.Lerp(CamSize,tempCalc,intensity);
                yield return null;
            }
            yield return new WaitForSeconds(time);
            while (CamSize != tempCamSize)
            {
               CamSize = Mathf.Lerp(CamSize, tempCamSize, intensity);
                yield return null;
            }
            CamSize = tempCamSize;
            inShake = false;
        }
    }



    public IEnumerator ShakeCamGroundFeeling(float intensity, float time)
    {
        if (inShake) yield break;
        inShake = true;
        CinemachineBasicMultiChannelPerlin perlin =
            StickCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        float elapsed = 0f;
        perlin.m_AmplitudeGain = intensity;
        perlin.m_FrequencyGain = 2f; 
        while (elapsed < time)
        {
            elapsed += Time.unscaledDeltaTime;
            perlin.m_AmplitudeGain = Mathf.Lerp(intensity, 0f, elapsed / time);
            yield return null;
        }
        perlin.m_AmplitudeGain = 0f;
        perlin.m_FrequencyGain = 0f;

        inShake = false;
    }
    //CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
    //    StickCam.GetCinemachineComponent < CinemachineBasicMultiChannelPerlin>();
    //cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
    //IntensityStart = intensity;
    //ShakeTimerTotal = time;
    //shakeTime = time;
}
