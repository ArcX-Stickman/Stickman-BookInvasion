using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VineStopper : MonoBehaviour
{
    public bool CannotHit;
    public GameObject Cutscene;
    public bool CanCounter;
    public Movement PlayerManager;
    public Animator Whip;
    public AttackSight MeleeRange;
    public Health StickHP;
    public float ATK_CD;
    private float LastWhip;
    public float DMG;
    public bool Hit;
    public int CountOfHits;
    public GameObject Tip;
    public TextMeshProUGUI TipText;
    public Animator TipAnim;
    public AudioSource AudioP;
    public AudioClip SFX;
    public PauseManager PauseManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        if(CountOfHits >= 5)
        {
            StartCoroutine(Tips());
            CountOfHits = 0;

        }



        if (MeleeRange.StickmanIsHERE)
        {
            if (Time.time > LastWhip + ATK_CD)
            {
                Hit = false;
                LastWhip = Time.time;
                Whip.SetTrigger("Whip");
                
            }
        }
        
       
    }

   public void CanCounterIstrue()
    {
        CanCounter = true;
    }

    public void NoCanCounterIstrue()
    {
        CanCounter = false;
    }


    public void Attack()
    {
        if (PlayerManager.CanCatch && MeleeRange.StickmanIsHERE)
        {
            CannotHit = true;
            Cutscene.gameObject.SetActive(true);
            Destroy(gameObject);
        }
        else
        {
            if (!CannotHit && !Hit && MeleeRange.StickmanIsHERE)
            {
                CountOfHits += 1;
                Hit = true;
                StickHP.PlayerHP -= DMG;
                PlayerManager.StickAnim.SetTrigger("Hurt");
                AudioP.PlayOneShot(SFX);
            }
        }
    }

    public IEnumerator Tips()
    {
        PauseManager.NoHUD = true;

        Tip.gameObject.SetActive(true);
        PauseManager.Pause();
        TipText.text = "Try Countering The Vine";
        yield return new WaitForSecondsRealtime(3);
        TipAnim.SetTrigger("GoBack");
        PauseManager.Resume();
        PauseManager.NoHUD = false;

    }



}
