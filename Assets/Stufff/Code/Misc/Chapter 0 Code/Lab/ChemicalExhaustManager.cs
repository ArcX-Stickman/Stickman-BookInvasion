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

    IEnumerator WaitForPlayer(Vision vol, float minWait)
    {
        float timer = 0f;

        // lil  delay
        while (timer < minWait)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        float failSafe = 6f;
        while (!vol.PlayerSpotted && failSafe > 0f)
        {
            failSafe -= Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    public IEnumerator PLatQTE()
    {
        AudioBassdrop.Stop();
        Smoke1.SetActive(true);
        yield return StartCoroutine(WaitForPlayer(PlayerVolume1, 0.5f));

        AudioP.PlayOneShot(Explosion);
        Explosion1.SetActive(true);
        Destroy(Vent1);


        yield return StartCoroutine(WaitForPlayer(PlayerVolume2, 0.1f));
        Smoke2.SetActive(true);

        yield return StartCoroutine(WaitForPlayer(PlayerVolume3, 0.1f));
        Smoke3.SetActive(true);

        yield return StartCoroutine(WaitForPlayer(PlayerVolume4, 0.6f));

        Explosion2.SetActive(true);
        AudioP.PlayOneShot(Explosion);
        Destroy(Vent2);

        yield return StartCoroutine(WaitForPlayer(PlayerVolume5, 0.01f));
        Smoke4.SetActive(true);

        yield return StartCoroutine(WaitForPlayer(PlayerVolume6, 0.1f));
        Smoke5.SetActive(true);

        yield return StartCoroutine(WaitForPlayer(PlayerVolume7, 0.6f));

        Explosion3.SetActive(true);
        AudioP.PlayOneShot(Explosion); 
        Destroy(Vent3);

        yield return StartCoroutine(WaitForPlayer(PlayerVolume8, 0.1f));
        Smoke6.SetActive(true);

        yield return StartCoroutine(WaitForPlayer(PlayerVolume9, 0.1f));
        Smoke7.SetActive(true);

        yield return StartCoroutine(WaitForPlayer(PlayerVolume10, 0.6f));

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
        yield return new WaitUntil(() => (Explosion7 == null));

        BGMusic.UnPause();
        stick.TimeManager.WantAudioSame = false;
        HeliAudio.enabled = true;

        yield break;
    }
}
