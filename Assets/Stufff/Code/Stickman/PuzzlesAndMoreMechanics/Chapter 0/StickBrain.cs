using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StickBrain : MonoBehaviour
{
    public float Health;
    public Animator Animator;
    public Slider HP_BAR;
    public GameObject FX;
    public Movement PlayerManager;
    public Animator MainCamAnim;
    public float Distance;
    public float MinDIS;
    public CameraScript CamScript;
    public LoadLevel LoadLevel;
    public bool ALR_DID;
    public Animator StickAnim;
    public AudioSource BG_MUSIC;
    public AudioSource Shoot_WhatTimeisIt_Theme;
    public AudioClip HitSFX;
    public AudioSource AudioP;


    // Start is called before the first frame update
    void Start()
    {
        HP_BAR.gameObject.SetActive(false);

        HP_BAR.maxValue = Health;

    }

    // Update is called once per frame
    void Update()
    {
        Distance = Vector2.Distance(transform.position,PlayerManager.transform.position);
        HP_BAR.value = Health;
        if(Distance <= MinDIS)
        {
            HP_BAR.gameObject.SetActive(true);
        }
        if(Health <= 0 && !ALR_DID)
        {
            Animator.SetTrigger("Awake");
            StickAnim.SetTrigger("DodgeLow");
            BG_MUSIC.gameObject.SetActive(false);
            BG_MUSIC.mute = true;
            Shoot_WhatTimeisIt_Theme.gameObject.SetActive(true) ;
            //BG_MUSIC.loop = false;
            AudioP.PlayOneShot(HitSFX);
            ALR_DID = true;

        }
        if(Health <= 0)
        {
            // prevent player from damaging the brain further
            PlayerManager.MeleeCD = 999999;
            PlayerManager.shootCooldown = 999999;
           // PlayerManager.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            ObjectPooler.SpawnObj(FX,transform.position,Quaternion.identity, ObjectPooler.PoolType.Partcles);
            Health -= 1;
            ObjectPooler.ReturnObjectToPool(collision.gameObject);
            MainCamAnim.ResetTrigger("ShakeMelee");
            Animator.SetTrigger("Hurt");
            CamScript.StartCoroutine(CamScript.ShakeCam(2.5f, 0.15f));


        }
        if (collision.CompareTag("StickMelee"))
        {
            ObjectPooler.SpawnObj(FX, transform.position, Quaternion.identity, ObjectPooler.PoolType.Partcles);
            Health -= 0.5f;
            PlayerManager.CurrentPunches += 1;
            MainCamAnim.ResetTrigger("ShakeMelee");
            Animator.SetTrigger("Hurt");
            CamScript.StartCoroutine(CamScript.ShakeCam(1.5f, 0.15f));


        }
    }

    public void ExtendOut()
    {
        CamScript.StartCoroutine(CamScript.StickBrainWakeUp_Event());
    }

    public void LoadScene()
    {
        PlayerManager.Checkpoint = 551;
        PlayerManager.SaveProgress();
        PlayerManager.LoadingSystem.LoadGameScene();

    }
}
