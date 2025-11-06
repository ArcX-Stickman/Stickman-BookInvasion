using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnPalenPHY : MonoBehaviour
{
    public AudioClip Victory;
    public AudioSource HypothesisTheme;
    public AudioSource BattleTheme;
    public GameObject Directions4Stun;
    public bool InTutorial;
    public GameObject Physics;
    public GameObject Palentology;
    public AudioSource AudioP;
    public AudioClip SFX;
    public TextMeshProUGUI TextTip;
    public bool Entered;
    public Animator HUDAnim;
    public bool NotLab;
    public bool OnlyOne;
    public bool NoBatttleTheme;
    // Start is called before the first frame update
    void Start()
    {
        if (Directions4Stun != null)
        {
            Directions4Stun.gameObject.SetActive(false);

        }


        Physics.gameObject.SetActive(false);
        Palentology.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (InTutorial && Entered)
            {
                StartCoroutine(DashTip());
            }

        }

        if(NotLab && OnlyOne)
        {
            if(Physics == null && !AlrPlayedJingle && !NoBatttleTheme)
            {
                AlrPlayedJingle = true;
                AudioP.PlayOneShot(Victory);
                HypothesisTheme.UnPause();
                BattleTheme.gameObject.SetActive(false);

            }
            else if(Palentology == null && !AlrPlayedJingle && !NoBatttleTheme)
            {
                AlrPlayedJingle = true;
                AudioP.PlayOneShot(Victory);
                HypothesisTheme.UnPause();
                BattleTheme.gameObject.SetActive(false);
            }
        }


        if (Entered && NotLab && !AlrPLayedMusic && !NoBatttleTheme)
        {
            AlrPLayedMusic = true;
            HypothesisTheme.Pause();
            BattleTheme.gameObject.SetActive(true);
        }

        if(NotLab && Physics == null && Palentology == null && ! AlrPlayedJingle && !NoBatttleTheme) 
        {
            AlrPlayedJingle = true;
            AudioP.PlayOneShot(Victory);
            HypothesisTheme.UnPause();
            BattleTheme.gameObject.SetActive(false);
        }
        
    }
    private bool AlrPlayedJingle;
    private bool AlrPLayedMusic;
    private bool SFXPlayed;
    public bool Spawned;
    public bool TipAlr;

    public IEnumerator Waiting()
    {
        yield return new WaitForSecondsRealtime(5);
        HUDAnim.SetTrigger("GoBack");

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {


            Entered = true;


            if (InTutorial)
            {
                if (!SFXPlayed)
                {
                    AudioP.PlayOneShot(SFX);
                    SFXPlayed = true;
                }
                if (!TipAlr)
                {
                    TipAlr = true;
                    Directions4Stun.gameObject.SetActive(true);
                    TextTip.text = "If Stunned Dodge, then Dash [SHIFT]";
                    StartCoroutine(Waiting());
                }
               
            }


            if (!Spawned)
            {
                Spawned = true;
                Physics.gameObject.SetActive(true);
                Palentology.gameObject.SetActive(true);

            }
            
            
        }
    }
    public IEnumerator DashTip()
    {
        
        if (!Directions4Stun.gameObject.activeSelf)
        {
            Directions4Stun.gameObject.SetActive(true);
            TextTip.text = "Dashing drains Energy";
            yield return new WaitForSecondsRealtime(2f); 
            TextTip.text = "Regain from Dodging or Fighting";
            yield return new WaitForSecondsRealtime(2f);
            HUDAnim.SetTrigger("GoBack");

            //Destroy(gameObject);
        }
        else
        {
            yield return new WaitForSecondsRealtime(5f);
            Directions4Stun.gameObject.SetActive(true);
            TextTip.text = "Dashing drains Energy";
            yield return new WaitForSecondsRealtime(2f);
            TextTip.text = "Regain from Dodging or Fighting";
            yield return new WaitForSecondsRealtime(2f);
            HUDAnim.SetTrigger("GoBack");

            //Destroy(gameObject);
        }
    }








}
