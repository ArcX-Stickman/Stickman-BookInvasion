using System.Collections;
using UnityEngine;

public class MoveTowardsPosition : MonoBehaviour
{
    [Header("Movement")]
    public Transform PlaceToGO;
    public bool WantToGO;
    public float Speed;
    public bool WantAcceleration;
    public bool StickBasedSpeed;
    public bool WantFollowYAxis;
    public bool FlipScale;
    public bool SlowDownIFPlayerFar;
    public float OriginalSpeed;
    public float MIN_DIS_ATTRIBUTE_FOR_REFERENCE;
    public bool DestoryOnReach;
    public Transform SecondDes;
    public bool WantSecondDisTrigger;
    public bool SecondDisNow;
    public float Distance;
    public float MinDisForStickMove;

    [Header("Booleans/Conditions")]
    public bool CanBeStoppedByCollision;
    public bool CanBeDodged;
    public bool CanBeDestroyedGround;
    public bool CanKillPlayer;
    public bool SloMo;
    public bool WantFx;
    public bool WantDestroyOnImpact;
    public bool FxHasAnimAndFXNeedTOBeFast;
    public bool WantShakingIntensity;
    public bool ShakeOnDestroy;
    public bool Grounded;
    public float DMG_Value;
    private bool Flipped;
    private bool alrShookCam;
    public bool WantMuiscSameSlo;

    [Header("Refer")]
    public Movement Stick;
    public AudioSource AudioP;
    public AudioClip SFX;
    public GameObject FX;
    public float IntensityDestruc_CamShake;

    [Header("Chapter 0 - Conditions")]
    public bool Chem;
    public Chemistry Chemistry;
    public GameObject ChemUI_Dodge;

    private void Start()
    {
        OriginalSpeed = Speed;
    }

    private void Update()
    {
        if (!gameObject.activeSelf) return;

        HandleMovement();
        if (SlowDownIFPlayerFar) SpeedManagerSlo();
        if (WantShakingIntensity && Grounded) StartCoroutine(ShakeLikeCRazYYY());
    }

    private void OnTriggerEnter2D(Collider2D collision) => HandleCollision(collision.gameObject);
    private void OnCollisionEnter2D(Collision2D collision) => HandleCollision(collision.gameObject);

    private void OnDestroy()
    {
        if (CanKillPlayer)
            StartCoroutine(Stick.CameraScript.ShakeCamGroundFeeling(80f, 0.3f));
    }

    // ?????????????????????????????
    // ?? MOVEMENT HANDLING
    // ?????????????????????????????
    private void HandleMovement()
    {
        Stick = FindAnyObjectByType<Movement>();
        float distanceToTarget = WantFollowYAxis
            ? Vector2.Distance(transform.position, PlaceToGO.position)
            : Vector2.Distance(transform.position, new Vector2(PlaceToGO.position.x, transform.position.y));

        Distance = distanceToTarget;
        float minDestroyDistance = MIN_DIS_ATTRIBUTE_FOR_REFERENCE;

        // Stick-based movement speed
        if (StickBasedSpeed)
        {
            Distance = Vector2.Distance(transform.position, Stick.transform.position);
            if (Distance <= MinDisForStickMove) Speed = Stick.MaxSpeed - 15.4f;
            else if (Distance >= 65) Speed = Stick.MaxSpeed + 13f;
            else Speed = Stick.MaxSpeed - 9.4f;
        }

        // Flip on movement start
        if (FlipScale && WantToGO && !Flipped)
        {
            Flipped = true;
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }

        // Move toward target
        if (WantToGO)
        {
            Vector2 targetPos = WantFollowYAxis
                ? PlaceToGO.position
                : new Vector2(PlaceToGO.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime);
        }

        // Check for reach conditions
        if (distanceToTarget <= minDestroyDistance && DestoryOnReach) Destroy(gameObject);
        if (distanceToTarget <= minDestroyDistance - 7 && SecondDes != null && SecondDisNow) PlaceToGO = SecondDes;
    }

    private IEnumerator ShakeLikeCRazYYY()
    {
        while (Grounded)
        {
            Stick.CameraScript.StartCoroutine(
                Stick.CameraScript.ShakeCamGroundFeeling(IntensityDestruc_CamShake, 0.1f)
            );
            yield return null;
        }
    }

    private void SpeedManagerSlo()
    {
        Vector2 directionToStick = (Stick.transform.position - transform.position).normalized;
        float dot = Vector2.Dot(transform.right, directionToStick);
        Speed = Mathf.Lerp(OriginalSpeed - 5f, OriginalSpeed + 80f, (dot + 1f) / 0.3f);
    }

    // ?????????????????????????????
    // ?? COLLISION HANDLING
    // ?????????????????????????????
    private void HandleCollision(GameObject obj)
    {
        string tag = obj.tag;

        // SPEC collision
        if (tag == "Spec" && CanBeStoppedByCollision)
        {
            PlayFXAndShake();
            GetComponent<DeactivateScriptOnCollision>().Deactivate();
            return;
        }

        // PLAYER collision
        if (tag == "Player" && CanKillPlayer)
        {
            HandlePlayerImpact();
            return;
        }

        // GROUND collision
        if (tag == "Ground")
        {
            Grounded = true;
            HandleGroundImpact();
        }
    }

    // ?????????????????????????????
    // ?? IMPACT HELPERS
    // ?????????????????????????????
    private void HandlePlayerImpact()
    {
        if (Chem)
        {
            Chemistry.Chem_Anim.CrossFade("Chem_KnockedOut", 0, 0);
            ChemUI_Dodge.SetActive(false);
        }

        bool playerDodged = CanBeDodged && (Stick.isDashing && !Chem || Stick.isSliding);
        if (playerDodged)
        {
            PlayFXAndShake();
            GetComponent<DeactivateScriptOnCollision>().Deactivate();
            return;
        }

        // Apply damage
        if (WantFx)
        {
            if (DMG_Value == 0)
            {
                Stick.StickHP.PlayerHP = 0;
                Stick.StickAnim.CrossFade("Dead", 0, 0);
            }
            else
            {
                Stick.StickHP.PlayerHP -= DMG_Value;
            }

            PlayFXAndShake(true);
        }
    }

    private void HandleGroundImpact()
    {
        if (Chem)
        {
            Chemistry.Chem_Anim.CrossFade("Chem_KnockedOut", 0, 0);
            ChemUI_Dodge.SetActive(false);
        }

        if (CanBeDestroyedGround)
        {
            PlayFXAndShake();
            if (CanBeDestroyedGround)
                GetComponent<DeactivateScriptOnCollision>().Deactivate();
        }
    }

    private void PlayFXAndShake(bool checkFastAnim = false)
    {
        if (!WantFx) return;

        if (AudioP && SFX)
            AudioP.PlayOneShot(SFX);

        if (!alrShookCam)
        {
            alrShookCam = true;
            Stick.CameraScript.StartCoroutine(
                Stick.CameraScript.ShakeCamGroundFeeling(IntensityDestruc_CamShake, 0.5f)
            );
        }

        GameObject boom = ObjectPooler.SpawnObj(FX, transform.position, Quaternion.identity, ObjectPooler.PoolType.Partcles);
        if (checkFastAnim && FxHasAnimAndFXNeedTOBeFast)
        {
            Animator fxAnim = boom.GetComponent<Animator>();
            if (fxAnim != null) fxAnim.speed *= 5;
        }

        if (WantDestroyOnImpact)
        {
            if(SloMo && WantMuiscSameSlo) Stick.TimeManager.ForceMusicSame = true;
            if (SloMo) Stick.TimeManager.SloMO();
            Destroy(gameObject);
        }
    }
}
