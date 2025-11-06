using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamFlee : MonoBehaviour
{

    public Movement Stick;
    public AudioSource AudioP;
    public AudioClip Fail;
    public bool Alr_Did;
    public AudioSource Music;
    private bool AlrStart;
    public GameObject UI_Fail;
    public GameObject PhysicsBook;
    private void Update()
    {
        if (AlrStart)
        {
            Stick.HeCanMove = false;
            Stick.Speed = 0;
            Stick.rb.velocity = Vector2.zero;
            Time.timeScale = 1;
            Stick.gameObject.isStatic = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Book"))
        {
            if(PhysicsBook != null && Stick.StickHP.PlayerHP > 0)
            {
                Alr_Did = true;
                Stick.StickAnim.CrossFade("Stick_Failed_Misson", 0, 0);
                AudioP.PlayOneShot(Fail);
                Stick.CameraScript.CamSize = 20;
                Music.enabled = false;
                StickMoveGlitch SMG = Stick.gameObject.GetComponent<StickMoveGlitch>();
                SMG.enabled = false;
                Stick.enabled = false;
                if (!AlrStart)
                {
                    StartCoroutine(WaitForBit());
                }
            }
          
           
        }
    }

    IEnumerator WaitForBit()
    {
        yield return new WaitForSecondsRealtime(2);
        Stick.enabled = false;
        UI_Fail.SetActive(true);
        Stick.StickAnim.enabled = false;
        yield return new WaitForSecondsRealtime(6);
        LoadingSystem lS = FindAnyObjectByType<LoadingSystem>();
        lS.LoadGameScene();
    }
}
