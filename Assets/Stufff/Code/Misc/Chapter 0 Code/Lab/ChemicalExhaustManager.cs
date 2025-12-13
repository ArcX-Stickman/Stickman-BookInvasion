using System.Collections;
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
    public AudioSource AudioBassdrop;
    public AudioClip Explosion;
    public AudioClip BassDrop;
    public AudioSource BGMusic;
    public AudioSource HeliAudio;

    public Vision PlayerVolume1;
    public Vision PlayerVolume2;
    public Vision PlayerVolume3;
    public Vision PlayerVolume4;
    public Vision PlayerVolume5;
    public Vision PlayerVolume6;
    public Vision PlayerVolume7;
    public Vision PlayerVolume8;
    public Vision PlayerVolume9;
    public Vision PlayerVolume10;
    public Vision PlayerVolume11;
    public Vision PlayerVolume12;
    public Vision PlayerVolume13;

    void Update()
    {
        if (!AlrRunning && Touched)
        {
            AlrRunning = true;
            StartCoroutine(PLatQTE());
        }
    }

    IEnumerator WaitForPlayer(Vision vol, float minWait, float maxWait)
    {
        float time = 0f;

        while (time < minWait)
        {
            time += Time.deltaTime;
            yield return null;
        }

        float elapsed = 0f;
        while (!vol.PlayerSpotted && elapsed < maxWait)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
    }


    public IEnumerator PLatQTE()
    {
        AudioBassdrop.Stop();
        Smoke1.SetActive(true);
        yield return StartCoroutine(WaitForPlayer(PlayerVolume1, 0.65f,1));

        AudioP.PlayOneShot(Explosion);
        Explosion1.SetActive(true);
        Destroy(Vent1);


        yield return StartCoroutine(WaitForPlayer(PlayerVolume2, 0.1f, 1));
        Smoke2.SetActive(true);

        yield return StartCoroutine(WaitForPlayer(PlayerVolume3, 0.1f, 1));
        Smoke3.SetActive(true);

        yield return StartCoroutine(WaitForPlayer(PlayerVolume4, 0.6f, 1));

        Explosion2.SetActive(true);
        AudioP.PlayOneShot(Explosion);
        Destroy(Vent2);

        yield return StartCoroutine(WaitForPlayer(PlayerVolume5, 0.01f, 1));
        Smoke4.SetActive(true);

        yield return StartCoroutine(WaitForPlayer(PlayerVolume6, 0.1f, 1));
        Smoke5.SetActive(true);

        yield return StartCoroutine(WaitForPlayer(PlayerVolume7, 0.6f, 1));

        Explosion3.SetActive(true);
        AudioP.PlayOneShot(Explosion); 
        Destroy(Vent3);

        yield return StartCoroutine(WaitForPlayer(PlayerVolume8, 0.1f, 1));
        Smoke6.SetActive(true);

        yield return StartCoroutine(WaitForPlayer(PlayerVolume9, 0.1f, 1));
        Smoke7.SetActive(true);

        yield return StartCoroutine(WaitForPlayer(PlayerVolume10, 0.6f, 1));

        Explosion4.SetActive(true);
        AudioP.PlayOneShot(Explosion);
        Movement stick = FindAnyObjectByType<Movement>();
        yield return new WaitUntil(() => stick.StickAnim.GetBool("Dive"));
        

        AudioBassdrop.PlayOneShot(BassDrop);
        HeliAudio.enabled = false;
        stick.TimeManager.WantAudioSame = true;
        stick.TimeManager.SloMO();
        BGMusic.Pause();
        if (AudioBassdrop.isPlaying)
        {
            Debug.Log("Yuh u just deaf my boy bassdrop is indeed workin");
        }
        AudioP.volume = 0.2f;
        AudioBassdrop.volume = 1;

        Explosion5.SetActive(true);
        AudioP.PlayOneShot(Explosion);
        Destroy(Vent5);
        yield return new WaitForSecondsRealtime(0.3f);

        Explosion6.SetActive(true);
        AudioP.PlayOneShot(Explosion);
        Destroy(Vent6);

        yield return new WaitForSecondsRealtime(0.3f);

        Explosion7.SetActive(true);
        AudioP.PlayOneShot(Explosion);
        Destroy(Vent7);
        //yield return new WaitUntil(() => (Explosion7 == null));
        yield return new WaitForSecondsRealtime(0.7f);

        BGMusic.UnPause();
        stick.TimeManager.WantAudioSame = false;
        HeliAudio.enabled = true;

        yield break;
    }
}
