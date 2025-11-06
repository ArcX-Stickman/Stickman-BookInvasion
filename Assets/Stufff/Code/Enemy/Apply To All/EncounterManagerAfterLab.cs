using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterManagerAfterLab : MonoBehaviour
{
    public Physics physics;
    public Scientist Scientist;
    public Geology geology;
    public AudioSource Hypothesis;
    public AudioSource BattleTheme;
    public AudioSource AudioP;
    public AudioClip WinSFX;
    public bool Fight;
    //public GameObject[] EnemyList;
    public bool PlayedWin = false;
    public bool PlayedBattleTheme = false;
    //private List<GameObject> enemyList;
    public Movement PlayerManager;



    // Start is called before the first frame update
    void Start()
    {
        if (PlayerManager.FinshedEncounterAfterLab)
        {
            Destroy(gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {

        if(physics != null)
        {
            if (physics.AttackSight.StickmanIsHERE && !Fight)
            {
                Fight = true;
                Scientist.AttackSight.StickmanIsHERE = true;
                geology.PlayerSpotted();
                Scientist.AttackSight.Force = true;
                geology.vision.PlayerSpotted = true;
                physics.AttackSight.Force = true ;
                Scientist.PlayerSpotted();

            }
        }
       
        if(Scientist != null)
        {
            if (Scientist.AttackSight.StickmanIsHERE && !Fight)
            {
                Fight = true;
                geology.PlayerSpotted();
                Scientist.PlayerSpotted();
                Scientist.AttackSight.Force = true;
                geology.vision.PlayerSpotted = true;
                physics.AttackSight.Force = true;
                physics.PlayerSpotted();
                physics.AttackSight.StickmanIsHERE = true;
            }
        
        }
        if(geology != null)
        {
            if (geology.vision.PlayerSpotted)
            {
                Fight = true;
                Scientist.PlayerSpotted();
                Scientist.AttackSight.StickmanIsHERE = true;
                physics.PlayerSpotted();
                Scientist.AttackSight.Force = true;
                geology.vision.PlayerSpotted = true;
                physics.AttackSight.Force = true;
                physics.AttackSight.StickmanIsHERE = true;
            }
        }
        
       
        if(physics == null && Scientist == null && geology == null)
        {
            Fight = false;
            Destroy(gameObject);
        }
        
        if(Fight && !PlayedBattleTheme)
        {
            PlayedBattleTheme = true;
            Hypothesis.gameObject.SetActive(false);
            BattleTheme.gameObject.SetActive(true);

        }
        if (!Fight && PlayedBattleTheme && !PlayedWin)
        {
            Fight = false;
            BattleTheme.gameObject.SetActive(false);
            PlayerManager.FinshedEncounterAfterLab = true;
            PlayerManager.SaveProgress();

            AudioP.PlayOneShot(WinSFX);
            PlayedWin = true;
            StartCoroutine(WaitForAsec());
        }

    }


    public IEnumerator WaitForAsec()
    {
        yield return new WaitForSecondsRealtime(2);
        Hypothesis.gameObject.SetActive(true );
    }
}
