using Cinemachine;

using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.Audio;
using UnityEngine.Events;


public class MoveToPositionV2 : MonoBehaviour

{



    [Header("Specification")]

    public bool Vec2;

    public bool Vec3;

    private Rigidbody2D rb;
    private Collider2D collider2d;

    public Transform[] AdditionalColliders;

    public string AutoAddAdditionals;



    [Header("Movement")]

    public Transform Position;

    public float Speed;

    public float GainOverTime_SPEED;

    public float MinDistance;

    public bool WantToAttachToPlayer;

    public Transform PlayerPos;

    [SerializeField] private float MinDis = 8f;

    private float Distance;



    [Header("Customization crap")]

    public bool FollowZ_Only;

    public bool FollowX_Only;

    public bool FollowY_Only;



    [Header("Optimization")]



    public bool DestroyReach;



    public bool CloneTech;



    public bool ObjectPooler;



    [Header("Activate")]



    public GameObject Activate;



    public CameraSukugaCuts Cam;



    public float CamShake;



    public CinemachineImpulseSource CamImpulseSource;



    public Animator ActivateAnim;



    public string AnimName;

    private float Og_Z;

    [Header("SFX")]



    public AudioSource AudioP;

    public AudioClip SFX;





    [Header("Traps and More")]

    public TrapsSukuga TrapSuk;
    public string[] Fake3DNonDamageTags = { "Ground" };
    public bool Fake3DNonDamageMustMatchZ;
    [SerializeField] private UnityEvent Event;

    private readonly List<Collider2D> fake3DOverlapResults = new List<Collider2D>();


    [Header("Hinderances")]
    public ChangeTransform Transformer;
    public bool WaitTilTranformerUnfroze;

    public void SetUp()

    {

        Og_Z = transform.position.z;

        if (FollowZ_Only)

        {

            Vector3 newP = new Vector3(transform.position.x, transform.position.y, Position.position.z);

            if (WantToAttachToPlayer)

            {

                newP = new Vector3(PlayerPos.position.x, PlayerPos.position.y, newP.z);

            }

            Transform trans = Instantiate(Position, newP, Quaternion.identity);

            Position = trans;



        }

        else if (WantToAttachToPlayer && !FollowZ_Only)

        {

            Vector3 newP = new Vector3(PlayerPos.position.x, PlayerPos.position.y, Position.position.z);

            Transform trans = Instantiate(Position, newP, Quaternion.identity);

            Position = trans;

        }



        // Nulls and stuff

        TrapSuk = gameObject.GetComponent<TrapsSukuga>();
        collider2d = gameObject.GetComponent<Collider2D>();



        



    }


    private void EventThing()
    {
        Event?.Invoke();
    }




    private bool Setup;

    void Update()

    {

        if (!Setup)

        {

            SetUp();

            Setup = true;

        }

        if(WaitTilTranformerUnfroze && Transformer != null && Transformer.Frozen)
        {
            return;
        }


        ActivationManager();







        Vector3 positionBeforeMove = transform.position;

        if (Vec2)

        {

            Vec2Man();

        }

        else

        {

            Vec3Man();

        }





        TrapsAndMore(positionBeforeMove, transform.position);



    }



    private bool OnceTr;

    [SerializeField] private float MessyDistance;

    public float XYRange = 1.5f;   
    public float ZRange = 2f;
    public float ZRange_Ground = 0.2f;


    public void TrapsAndMore(Vector3 positionBeforeMove, Vector3 positionAfterMove)
    {
        if (TrapSuk == null || TrapSuk.DMGed || OnceTr) return;

        Transform hitTarget = null;
        bool hitPlayer = false;
        float hitTime = float.PositiveInfinity;

        if (PlayerPos != null)
        {
            RegisterFake3DHit(PlayerPos, true, positionBeforeMove, positionAfterMove, ref hitTarget, ref hitPlayer, ref hitTime);
        }

        RegisterOverlappingFake3DNonDamageHits(positionBeforeMove, positionAfterMove, ref hitTarget, ref hitPlayer, ref hitTime);


        // Tree / additional objects
        if (AdditionalColliders != null)
        {
            foreach (Transform t in AdditionalColliders)
            {
                if (t == null) continue;

                RegisterFake3DHit(t, false, positionBeforeMove, positionAfterMove, ref hitTarget, ref hitPlayer, ref hitTime);
            }
        }

        if (hitTarget == null) return;

        if (hitPlayer)
        {
            //Debug.Log("Fake 3D player hit");

            if (TrapSuk.TryDMG_STUFF())
            {
                OnceTr = true;
            } 
        }
        else
        {
            //Debug.Log("Fake 3D ground hit");

            if (TrapSuk.TryNONDMG_STUFF())
            {
                OnceTr = true;
            }
        }
    }


    private void RegisterFake3DHit(
     Transform target,
     bool isPlayer,
     Vector3 positionBeforeMove,
     Vector3 positionAfterMove,
     ref Transform hitTarget,
     ref bool hitPlayer,
     ref float hitTime)
    {
        float targetHitTime;

        if (!Fake3DCollision(target, isPlayer, positionBeforeMove, positionAfterMove, out targetHitTime))
            return;

        RegisterHitCandidate(
            target,
            isPlayer,
            targetHitTime,
            ref hitTarget,
            ref hitPlayer,
            ref hitTime
        );
    }

    private void RegisterHitCandidate(Transform target, bool isPlayer, float targetHitTime, ref Transform hitTarget, ref bool hitPlayer, ref float hitTime)
    {
        if (hitPlayer && !isPlayer) return;

        if (isPlayer && !hitPlayer)
        {
            hitTarget = target;
            hitPlayer = true;
            hitTime = targetHitTime;
            return;
        }

        if (targetHitTime > hitTime) return;

        if (Mathf.Approximately(targetHitTime, hitTime)) return;

        hitTarget = target;
        hitPlayer = isPlayer;
        hitTime = targetHitTime;
    }

    private void RegisterOverlappingFake3DNonDamageHits(Vector3 positionBeforeMove, Vector3 positionAfterMove, ref Transform hitTarget, ref bool hitPlayer, ref float hitTime)
    {
        if (collider2d == null) return;

        fake3DOverlapResults.Clear();

        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter();

        collider2d.OverlapCollider(filter, fake3DOverlapResults);

        foreach (Collider2D targetCollider in fake3DOverlapResults)
        {
            if (targetCollider == null || targetCollider == collider2d) continue;
            if (IsOwnCollider(targetCollider)) continue;
            if (!IsFake3DNonDamageCollider(targetCollider)) continue;
            if (IsPlayerCollider(targetCollider)) continue;

            if (Fake3DNonDamageMustMatchZ)
            {
                RegisterFake3DHit(targetCollider.transform, false, positionBeforeMove, positionAfterMove, ref hitTarget, ref hitPlayer, ref hitTime);
            }
            else
            {
                RegisterFake3DHit(
                    targetCollider.transform,
                    false,
                    positionBeforeMove,
                    positionAfterMove,
                    ref hitTarget,
                    ref hitPlayer,
                    ref hitTime
                );
            }
        }
    }

    private bool IsFake3DNonDamageCollider(Collider2D targetCollider)
    {
        if (Fake3DNonDamageTags == null || Fake3DNonDamageTags.Length == 0)
        {
            return targetCollider.CompareTag("Ground");
        }

        foreach (string tagName in Fake3DNonDamageTags)
        {
            if (string.IsNullOrEmpty(tagName)) continue;
            if (targetCollider.CompareTag(tagName)) return true;
        }

        return false;
    }

    private bool IsPlayerCollider(Collider2D targetCollider)
    {
        if (PlayerPos == null) return false;

        return targetCollider.transform == PlayerPos || targetCollider.transform.IsChildOf(PlayerPos);
    }

    private bool IsOwnCollider(Collider2D targetCollider)
    {
        return targetCollider.transform == transform || targetCollider.transform.IsChildOf(transform);
    }

    private bool Fake3DCollision(
     Transform target,
     bool isPlayer,
     Vector3 positionBeforeMove,
     Vector3 positionAfterMove,
     out float hitTime)
    {
        hitTime = 1f;

        if (!FakeXYCollision(target))
            return false;


        // Different depth rules:
        // Player = forgiving
        // Ground = strict
        float currentZRange = isPlayer ? ZRange : ZRange_Ground;


        if (FollowZ_Only)
        {
            return ZOnlyCollision(
                positionBeforeMove.z,
                positionAfterMove.z,
                target.position.z,
                currentZRange,
                out hitTime
            );
        }


        float zDistance = Mathf.Abs(positionAfterMove.z - target.position.z);

        return zDistance <= currentZRange;
    }

    private bool FakeXYCollision(Transform target)
    {
        Collider2D targetCollider = target.GetComponent<Collider2D>();

        if (targetCollider == null)
        {
            targetCollider = target.GetComponentInChildren<Collider2D>();
        }

        if (collider2d != null && targetCollider != null)
        {
            ColliderDistance2D distance = collider2d.Distance(targetCollider);

            return distance.isOverlapped;
        }

        float xyDistance = Vector2.Distance(transform.position, target.position);

        return xyDistance <= XYRange;
    }

    private bool ZOnlyCollision(float startZ,float endZ,float targetZ,float range,out float hitTime)
    {
        hitTime = 1f;

        float minZ = Mathf.Min(startZ, endZ) - range;
        float maxZ = Mathf.Max(startZ, endZ) + range;

        if (targetZ < minZ || targetZ > maxZ) return false;

        float zDelta = endZ - startZ;

        if (Mathf.Approximately(zDelta, 0f))
        {
            hitTime = 0f;
            return true;
        }

        float enterZ = zDelta > 0f ? targetZ - range : targetZ + range;

        hitTime = Mathf.Clamp01((enterZ - startZ) / zDelta);

        return true;
    }


    private bool Once;

    private void ActivationManager()

    {



        if (Dis() <= MinDistance && !Once)

        {

            if (ActivateAnim != null)

            {

                ActivateAnim.CrossFade("AnimName", 0, 0);

            }

            else

            {

               // Debug.LogError("Animation missing is this normal??" + " Called from " + gameObject.name);



            }

            if (CamShake > 0 && Cam != null)

            {

                Cam.CameraShake(CamImpulseSource, CamShake);

            }

            else

            {

              //  Debug.LogError("no cam shake ok?? " + gameObject.name);

            }

            if (Activate.gameObject != null)

            {

                Activate.gameObject.SetActive(true);



            }

            else

            {

               // Debug.LogError("Gameobject missing is this normal??" + " Called from " + gameObject.name);

            }

            if (AudioP != null && SFX != null)

            {

                AudioP.PlayOneShot(SFX);

            }

            else

            {

                //Debug.LogError("Audio missing is this normal??" + " Called from " + gameObject.name);



            }

            Once = true;



        }



    }





    public void Vec2Man()

    {

        if (Event != null && Dis() <= MinDistance)
        {
            EventThing();
        }

        Vector3 Posfake = new Vector3 (Position.position.x,Position.position.y, Og_Z);

        if (Dis() >= MinDistance)

        {



            transform.position = Vector3.MoveTowards(transform.position, Posfake, Speed * Time.deltaTime);

        }

        else if (Dis() <= MinDistance && DestroyReach)

        {

            //ObjectPooler ob = FindAnyObjectByType<ObjectPooler>();



            if (CloneTech && gameObject.name.Contains("(Clone)"))

            {

                if (FollowZ_Only)

                {

                    Destroy(Position.gameObject);



                }

                Destroy(gameObject);



            }

            else if (!CloneTech)

            {

                if (FollowZ_Only)

                {

                    Destroy(Position.gameObject);



                }

                Destroy(gameObject);



            }

        }

    }



    public void Vec3Man()

    {
        if (Event != null && Dis() <= MinDistance)
        {
            EventThing();
        }
        if (Dis() >= MinDistance)

        {

            transform.position = Vector3.MoveTowards(transform.position, Position.position, Speed * Time.deltaTime);

        }

        else if (Dis() <= MinDistance && DestroyReach)

        {

            //ObjectPooler ob = FindAnyObjectByType<ObjectPooler>();



            if (CloneTech && gameObject.name.Contains("Clone"))

            {

                if (FollowZ_Only && Position.gameObject.name.Contains("Clone"))

                {

                    Destroy(Position.gameObject);



                }

                Destroy(gameObject);



            }

            else if (!CloneTech)

            {

                if (FollowZ_Only && Position.gameObject.name.Contains("Clone"))

                {

                    Destroy(Position.gameObject);



                }

                Destroy(gameObject);



            }

        }


    }





    private float Dis()

    {

        float temp;
        if (Position == null) return 0;


        if (Vec2)

        {

            temp = Vector2.Distance(transform.position, Position.position);

        }

        else

        {

            temp = Vector3.Distance(transform.position, Position.position);





        }



        return temp;

    }

}

