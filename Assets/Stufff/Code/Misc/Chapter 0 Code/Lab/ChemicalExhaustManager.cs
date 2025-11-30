using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemicalExhaustManager : MonoBehaviour
{
    public bool Touched;
    private bool AlrRunning;

    public GameObject Explosion1;
    public GameObject Explosion2;
    public GameObject Explosion3;
    public GameObject Explosion4;
    public GameObject Explosion5;
    public GameObject Explosion6;
    public GameObject Explosion7;

    public GameObject Vent1;
    public GameObject Vent2;
    public GameObject Vent3;
    public GameObject Vent4;
    public GameObject Vent5;
    public GameObject Vent6;
    public GameObject Vent7;
    public GameObject Vent8;

    public GameObject Smoke1;
    public GameObject Smoke2;
    public GameObject Smoke3;
    public GameObject Smoke4;
    public GameObject Smoke5;
    public GameObject Smoke6;
    public GameObject Smoke7;
    public GameObject Smoke8;
    public GameObject Smoke9;

    public AudioSource AudioP;
    public AudioClip Explosion;
    public Vision PlayerVolume;
    private bool AlrRunningQTE;
    public AudioClip BassDrop;
    public AudioSource BGMusic;
    
    void Update()
    {
        if(!AlrRunning && Touched)
        {
            AlrRunning = true;
            StartCoroutine(PLatQTE());
        }
       
    }

    public IEnumerator PLatQTE()
    {
        Smoke1.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1.2f);
        AudioP.PlayOneShot(Explosion);
        Explosion1.gameObject.SetActive(true);
        Destroy(Vent1);
        yield return new WaitForSecondsRealtime(0.2f);
        Smoke2.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        Smoke3.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1F);
        Explosion2.gameObject.SetActive(true);
        AudioP.PlayOneShot(Explosion);

        Destroy(Vent2);
        yield return new WaitForSecondsRealtime(0.3f);
        Smoke4.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.1f);
        Smoke5.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        Explosion3.gameObject.SetActive(true);
        AudioP.PlayOneShot(Explosion);

        Destroy(Vent3);
        yield return new WaitForSecondsRealtime(0.1f);
        Smoke6.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        Smoke7.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        Explosion4.gameObject.SetActive(true);
        AudioP.PlayOneShot(Explosion);
        Movement stick = FindAnyObjectByType<Movement>();
        AudioP.PlayOneShot(BassDrop);
        stick.TimeManager.WantAudioSame = true;
        stick.TimeManager.SloMO();
        while (stick.Grounded)
        {
            yield return null;
        }
        //yield return new WaitForSecondsRealtime(1.2f);

        Explosion5.gameObject.SetActive(true);
        AudioP.PlayOneShot(Explosion);
        Destroy(Vent5);
        BGMusic.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(0.3f);

        Explosion6.gameObject.SetActive(true);
        AudioP.PlayOneShot(Explosion);
        Destroy(Vent6);
        yield return new WaitForSecondsRealtime(0.2f);

        Explosion7.gameObject.SetActive(true);
        AudioP.PlayOneShot(Explosion);

        Destroy(Vent7);
        BGMusic.gameObject.SetActive(true);
        stick.TimeManager.WantAudioSame = false;



        yield break;
    }

    
}
