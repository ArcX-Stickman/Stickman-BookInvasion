using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EmbrySukugaCut : MonoBehaviour
{

    public StickCinematicVer_BossFight StickManager;
    public Animator EnemyAnim;
    public AudioClip PunchImpact;
    public AudioSource PunchAudioP;

    public float DistanceFromPlayer;
    public float MinDisPlayer;
    public Transform ImpactFX;
    public GameObject ImpactFX_OBJ;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DistanceFromPlayer = Vector2.Distance(transform.position,StickManager.gameObject.transform.position);


        RunImportant();   
    }


    public void RunImportant()
    {
        // Animations:
        AnimationManager();
    }

    private float LastSFX;
    private float SFXCD = 0.2f;
    public void AnimationManager()
    {

        if (StickManager.Attack && DistanceFromPlayer <= MinDisPlayer)
        {

            if (StickManager.PunchA)
            {
                EnemyAnim.CrossFade("Punch", 0, 0);

            }
            else if(StickManager.KickA)
            {
                EnemyAnim.CrossFade("Kick", 0, 0);
            }

           
        }

    }


    public void AnimationSFX_Punch()
    {
        if (StickManager.Attack && DistanceFromPlayer <= MinDisPlayer)
        {
            if (Time.time > LastSFX + SFXCD)
            {
                LastSFX = Time.time;
                PunchAudioP.pitch = UnityEngine.Random.Range(0.9f, 1.3f);
                PunchAudioP.PlayOneShot(PunchImpact);
               
                
                AudioClip rnd = StickManager.PunchHit[UnityEngine.Random.Range(0, StickManager.PunchHit.Length)];

                PunchAudioP.PlayOneShot(rnd);

                CameraSukugaCuts CSC = FindAnyObjectByType<CameraSukugaCuts>();
                CSC.CameraShake(gameObject.GetComponent<CinemachineImpulseSource>(), 1);

                GameObject FX = Instantiate(ImpactFX_OBJ.gameObject,ImpactFX.transform.position,Quaternion.identity);
            }




        }
           
    }

    


}
