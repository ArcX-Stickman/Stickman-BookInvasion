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
    public PlayerStatusInGame PSIG;
    public Transform Stick;
    public Transform BossTransform;

    public bool inBattle;
    public float BattleCamSize;
    // Start is called before the first frame update

    // Helicopter Chapter 0 - Notes
    // Glass QTE Casm size: 35.3
    void Start()
    {
        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
        brain.m_DefaultBlend.m_Time = 0f;
        StickCam = GetComponent<CinemachineVirtualCamera>();
        if (OG_CAM_SIZE == 0)
        {
            OG_CAM_SIZE = CamSize;

        }
        transposer = StickCam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private float elapsedTime;
    public float durationofDeath = 4;
    private bool ALr_StartedDeath;
    private bool isDoingSomething;



    private void OnEnable()
    {
        if (StickManager.StickHP.PlayerHP > 0)
        {
            StopAllCoroutines();

        }
    }
    void Update()
    {


        if (StickManager.StickHP.PlayerHP > 0)
        {
            StickCam.m_Lens.OrthographicSize = CamSize;
            StickCam.m_Lens.FieldOfView = CamSize + AdditivePerspec;

        }
        else if (StickManager.StickHP.PlayerHP <= 0 && !ALr_StartedDeath)
        {
            StickCam.m_Lens.OrthographicSize = CamSize;
            StickCam.m_Lens.FieldOfView = CamSize;
            ALr_StartedDeath = true;
            StartCoroutine(DeathCam());

        }
        if (Psych_Cutscene != null)
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


            //BattleCam_Manager();
        }



        if (StickCam.m_Follow == null)
        {
            StickCam.m_Follow = StickManager.gameObject.GetComponent<Transform>();
        }





    }

    private Transform MidPointBasedPosition;
    private Transform StickPOS;
    public bool HoldNeeeded;
    [SerializeField] private Transform InviMidPoint;
    public void BossCamManager()
    {
        //if(StickPOS == null)
        //{
        //    StickPOS = StickManager.transform;
        //}
        //Vector3 midpoint = (StickPOS.position + BossTransform.position) / 2f; // ts lowkey bad code performance i think so change later haha in later update rn it jus gotta work Zehahaahha

        //InviMidPoint.position = midpoint;
        //Stick = InviMidPoint;

    }

    public void BattleCam_Manager()
    {
        if (StickManager.StickHP.PlayerHP > 0 && inBattle)
        {
            if (CamSize != BattleCamSize)
            {
                StartCoroutine(ZoomIn(BattleCamSize, 1.5f));
            }
        }
        else if (StickManager.StickHP.PlayerHP > 0 && !inBattle)
        {
            StartCoroutine(ZoomIn(OG_CAM_SIZE, 1.5f));

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


    public IEnumerator ZoomCam(float Size, float Duration)
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
        while (CamSize < 3492)
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
        if (inZoomCour) yield break;
        PriorCamSizeToZoom = CamSize;
        inZoomCour = true;
        while (CamSize != Size)
        {
            CamSize = Mathf.Lerp(CamSize, Size, time);
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


    public IEnumerator ZoomOut(float time)
    {
        if (inZoomOutCour) yield break;
        inZoomOutCour = true;
        while (CamSize != PriorCamSizeToZoom)
        {
            CamSize = Mathf.Lerp(CamSize, PriorCamSizeToZoom, time);
            yield return null;

        }
        inZoomOutCour = false;
        yield return null;
    }


    public IEnumerator ShakeCam(float intensity, float time)
    {
        if (notlocked) yield break;
        if (inShake) yield break;
        if (!inShake)
        {
            inShake = true;
            tempCamSize = CamSize;
            float tempCalc = CamSize - intensity;
            while (CamSize != tempCalc)
            {
                CamSize = Mathf.Lerp(CamSize, tempCalc, intensity);
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
    bool SpeicalShake;
    public IEnumerator ShakeCamDirection(float intensity, float time, Transform reference, bool Flip)
    {
        if (SpeicalShake) yield break;
        SpeicalShake = true;

        var vcam = StickCam;
        var transposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();

        Vector3 originalOffset = transposer.m_TrackedObjectOffset;

        // direction from reference to camera target
        Transform target = FindObjectOfType<Movement>().transform;
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

            // sharp anime falloff (fast hit, quick decay)
            float strength = intensity * (1f - t * t);

            transposer.m_TrackedObjectOffset = originalOffset + worldDir * (strength * 1.3f);

            yield return null;
        }

        elapsed = 0f;


        float timeOut = 0.2f;
        float timer4Out = 0;

        while (HoldNeeeded && timer4Out < timeOut)
        {
            if (!HoldNeeeded)
            {
                Debug.Log("Camera hold wasnt needed");
                break;
            }
            timer4Out += Time.unscaledDeltaTime;
            elapsed += Time.unscaledDeltaTime;

            float t = elapsed / time;
            float strength = intensity * (1f - t * t);

            transposer.m_TrackedObjectOffset = originalOffset + worldDir * (strength * 0.3f);

            if(timer4Out >= timeOut)
            {
                Debug.Log("Camera hold Timed Out");
                break;
            }


            yield return null;
        }

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

    public IEnumerator ShakeCamDirection_Vertical(float intensity, float time, bool Up,Transform reference)
    {
        if (SpeicalShake) yield break;
        SpeicalShake = true;

        var vcam = StickCam;
        var transposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();

        Vector3 originalOffset = transposer.m_TrackedObjectOffset;

        // direction from reference to camera target
        Transform target = FindObjectOfType<Movement>().transform;
        float dir = 0;

        if (Up)
        {
            dir = 1;
        }
        else
        {
            dir = -1;
        }

        Vector3 worldDir = reference.up * dir;
       
        worldDir.Normalize();
        

        float elapsed = 0f;

        

        while (elapsed < time)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = elapsed / time;

            // sharp anime falloff (fast hit, quick decay)
            float strength = intensity * (1f - t * t);

            transposer.m_TrackedObjectOffset = originalOffset + worldDir * (strength * 1.3f);

            yield return null;
        }

        elapsed = 0f;


        float timeOut = 0.2f;
        float timer4Out = 0;

        while (HoldNeeeded && timer4Out < timeOut)
        {
            if (!HoldNeeeded)
            {
                Debug.Log("Camera hold wasnt needed");
                break;
            }
            timer4Out += Time.unscaledDeltaTime;
            elapsed += Time.unscaledDeltaTime;

            float t = elapsed / time;
            float strength = intensity * (1f - t * t);

            transposer.m_TrackedObjectOffset = originalOffset + worldDir * (strength * 0.3f);

            if (timer4Out >= timeOut)
            {
                Debug.Log("Camera hold Timed Out");
                break;
            }


            yield return null;
        }

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

    //CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
    //    StickCam.GetCinemachineComponent < CinemachineBasicMultiChannelPerlin>();
    //cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
    //IntensityStart = intensity;
    //ShakeTimerTotal = time;
    //shakeTime = time;

    // new verion of zooming cuz the other 3 are ste

    private Coroutine activeZoom;
    public void ZoomTo(float targetSize, float duration)
    {
        // Stop any zoom
        if (activeZoom != null)
        {
            StopCoroutine(activeZoom);
        }

        activeZoom = StartCoroutine(ZoomCoroutine(targetSize, duration));
    }

    private IEnumerator ZoomCoroutine(float targetSize, float duration)
    {
        MainCam.orthographic = true;

        float startSize = StickCam.m_Lens.OrthographicSize;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            CamSize = Mathf.Lerp(startSize, targetSize, elapsed / duration);
            StickCam.m_Lens.OrthographicSize = CamSize;
            yield return null;
        }

        CamSize = targetSize;
        StickCam.m_Lens.OrthographicSize = CamSize;

        activeZoom = null;
    }


}
