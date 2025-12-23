using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LucidStateMusic : MonoBehaviour
{
    public GameObject Phase1Enders;
    public AudioSource Phase1;
    public AudioSource Phase2;
    public AudioSource Phase3;
    public GameObject Phase2Ender;
    public GameObject Phase2Start;
    public BattleManager BattleManager1;
    public BattleManager BattleManager2;
    public GameObject Rain;
    private bool AlrPlayedThunder;
    public GameObject Transition;
    public GameObject Spawners;
    public AudioSource AudioP;
    public AudioClip Thunder;
    public Animator BG;
    private void Start()
    {
        Spawners.gameObject.SetActive(false);

    }
    private void Update()
    {
        if (BattleManager1.AllEnemiesDefeated() == true)
        {
            Phase1.volume = 0;
            Phase2.volume = 1;
            if(Phase2Start != null)
            {
                Phase2Start.gameObject.SetActive(true);
            }
        }
        if (BattleManager2.AllEnemiesDefeated())
        {
            Spawners.gameObject.SetActive(true);
            if (!AlrPlayedThunder)
            {
                BG.CrossFade("LucidDreamDarkening 0", 0, 0);
                AudioP.PlayOneShot(Thunder);
                AlrPlayedThunder = true;
                Transition.gameObject.SetActive(true);
                Rain.gameObject.SetActive(true);
            }
        }
    }


}    

