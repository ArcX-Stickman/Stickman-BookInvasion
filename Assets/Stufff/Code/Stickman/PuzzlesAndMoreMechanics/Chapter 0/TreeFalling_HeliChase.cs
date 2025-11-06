using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFalling_HeliChase : MonoBehaviour
{
    public float DistanceFromPlayer;
    public float MinDis;
    public Animator Anim;
    public Movement PlayerManager;
    public bool Done;
    public BoxCollider2D bxcd;
    public AudioSource AudioP;
    public AudioClip Smash;
    private bool AlrSFX;
    public GameObject Debris;
    public AudioClip Explosion;
    private bool AlrPlaySound;
    public GameObject ExplosionSprite;
    public bool NoSloMo;


    private void Start()
    {
        if(Debris != null)
        {
            Debris.gameObject.SetActive(false);

        }
    }
    void Update()
    {
        Vector2 FakeStick = new Vector2 (PlayerManager.transform.position.x, transform.position.y);
        DistanceFromPlayer = Vector2.Distance(transform.position, FakeStick);
        if (DistanceFromPlayer <= MinDis)
        {
            if(Debris != null)
            {
                Debris.gameObject.SetActive(true);

            }
        }
    }

    public void TriggerSloMO()
    {
        if (!NoSloMo)
        {
            PlayerManager.TimeManager.SloMO();

        }
    }

    public void DoneTrigger()
    {
        // Animation trigger at the end
        Done = true;
        StartCoroutine(WaitForPlayerLeave());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !Done && !PlayerManager.isSliding && !PlayerManager.isDashing)
        {
            if (!AlrSFX)
            {
                AlrSFX = true;
                AudioP.PlayOneShot(Smash);
            }
            PlayerManager.rb.constraints = RigidbodyConstraints2D.FreezePosition;
            PlayerManager.StickCollider.enabled = false;
            PlayerManager.rb.bodyType = RigidbodyType2D.Kinematic;
            PlayerManager.StickAnim.CrossFade("Dead", 0, 0);
            PlayerManager.StickHP.PlayerHP = 0;
        }
        if (collision.CompareTag("Spec"))
        {
            if(ExplosionSprite != null)
            {
                PlayerManager.CameraScript.StartCoroutine(PlayerManager.CameraScript.ShakeCamGroundFeeling(45.7f, 0.3f));
                GameObject booom = Instantiate(ExplosionSprite,transform.position,Quaternion.identity);
            }
            if (!AlrPlaySound)
            {
                AlrPlaySound = true;
                AudioP.PlayOneShot(Explosion);
            }
            Destroy(collision.gameObject);
            Anim.SetTrigger("Fall");

        }


    }



    public IEnumerator WaitForPlayerLeave()
    {
        while(PlayerManager.isSliding || PlayerManager.isDashing)
        {
            yield return null;
        }
        bxcd.isTrigger = false;
        yield break;
    }
}
