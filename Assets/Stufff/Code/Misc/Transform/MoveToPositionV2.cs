using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MoveToPositionV2 : MonoBehaviour
{

    [Header("Specification")]
    public bool Vec2;
    public bool Vec3;
    private Rigidbody2D rb;

    [Header("Movement")]
    public Transform Position;
    public float Speed;
    public float GainOverTime_SPEED;
    public float MinDistance;
    public bool WantToAttachToPlayer;
    public Transform PlayerPos;
    [SerializeField]private float MinDis = 8f;
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

    [Header("SFX")]

    public AudioSource AudioP;
    public AudioClip SFX;


    [Header("Traps and More")]
    public TrapsSukuga TrapSuk;

    public void SetUp()
    {
        if (FollowZ_Only)
        {
            Vector3 newP= new Vector3(transform.position.x,transform.position.y, Position.position.z);
            if (WantToAttachToPlayer)
            {
                newP = new Vector3(PlayerPos.position.x,PlayerPos.position.y, newP.z);
            }
            Transform trans = Instantiate(Position,newP, Quaternion.identity);
            Position = trans;

        }
        else if(WantToAttachToPlayer && !FollowZ_Only)
        {
            Vector3 newP = new Vector3(PlayerPos.position.x, PlayerPos.position.y, Position.position.z);
            Transform trans = Instantiate(Position, newP, Quaternion.identity);
            Position = trans;
        }

        // Nulls and stuff
        TrapSuk = gameObject.GetComponent<TrapsSukuga>();



    }



    private bool Setup;
    void Update()
    {
        if (!Setup)
        {
            SetUp();
            Setup = true;
        }

        ActivationManager();



        if (Vec2)
        {
            Vec2Man();
        }
        else
        {
            Vec3Man();
        }


        TrapsAndMore();
        
    }

    private bool OnceTr;
    public void TrapsAndMore()
    {
        if (PlayerPos == null) return;
        Distance = Vector3.Distance(transform.position,PlayerPos.position);
        if (OnceTr) return;
        if (Distance <= MinDis)
        {
            if(TrapSuk != null)
            {
                TrapSuk.DMG_STUFF();
                OnceTr = true;
                
            }
        }
    }

    private bool Once;
    private void ActivationManager()
    {

        if(Dis() <= MinDistance &&!Once)
        {
            if(ActivateAnim != null)
            {
                ActivateAnim.CrossFade("AnimName",0,0);
            }
            else
            {
                Debug.LogError("Animation missing is this normal??" + " Called from " + gameObject.name);

            }
            if (CamShake > 0 && Cam != null)
            {
                Cam.CameraShake(CamImpulseSource, CamShake);
            }
            else
            {
                Debug.LogError("no cam shake ok?? " + gameObject.name);
            }
            if(Activate.gameObject != null)
            {
                Activate.gameObject.SetActive(true);

            }
            else
            {
                Debug.LogError("Gameobject missing is this normal??" + " Called from " + gameObject.name);
            }
            if (AudioP != null && SFX != null)
            { 
                AudioP.PlayOneShot(SFX);
            }
            else
            {
                Debug.LogError("Audio missing is this normal??" + " Called from " + gameObject.name);

            }
            Once = true;

        }

    }


    public void Vec2Man()
    {
        if(Dis() >= MinDistance)
        {
            
            transform.position = Vector2.MoveTowards(transform.position, Position.position, Speed * Time.deltaTime);
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
