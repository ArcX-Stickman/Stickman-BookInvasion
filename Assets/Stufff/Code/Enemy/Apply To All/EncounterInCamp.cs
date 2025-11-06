using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterInCamp : MonoBehaviour
{

    public Geology geology;
    public Astrology astrology;
    public Physics physics;
    public Element elements;
    public Neuroscience neuroscience;
    public AudioSource RegularMusic;
    public AudioSource BattleTheme;
    public AudioClip Jingle;
    public AudioSource AudioP;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    private bool AlrPLayedMusic;
    private bool AlrPlayedJingle;

    // Update is called once per frame
    void Update()
    {

        // this bout to be messy :O

        if (geology.vision.PlayerSpotted)
        {
            BattleTheme.gameObject.SetActive(true);
            RegularMusic.gameObject.SetActive(false);
            astrology.PlayerSpotted();
            physics.PlayerSpotted();
            elements.PublicFollowPlayer();
            neuroscience.PlayerSpotted();
        }
        else if (astrology.RangeManager.PlayerSpotted)
        {
            BattleTheme.gameObject.SetActive(true);
            RegularMusic.gameObject.SetActive(false);
            astrology.PlayerSpotted();
            physics.PlayerSpotted();
            elements.PublicFollowPlayer();
            neuroscience.PlayerSpotted();
            geology.vision.PlayerSpotted = true;
        }
        else if (physics.vision.PlayerSpotted)
        {
            BattleTheme.gameObject.SetActive(true);
            RegularMusic.gameObject.SetActive(false);
            astrology.PlayerSpotted();
            physics.PlayerSpotted();
            elements.PublicFollowPlayer();
            neuroscience.PlayerSpotted();
            geology.vision.PlayerSpotted = true;
        }
        else if (elements.vision.PlayerSpotted)
        {
            BattleTheme.gameObject.SetActive(true);
            RegularMusic.gameObject.SetActive(false);
            astrology.PlayerSpotted();
            physics.PlayerSpotted();
            elements.PublicFollowPlayer();
            neuroscience.PlayerSpotted();
            geology.vision.PlayerSpotted = true;
        }
        else if (neuroscience.AttackSight.StickmanIsHERE)
        {
            BattleTheme.gameObject.SetActive(true);
            RegularMusic.gameObject.SetActive(false);
            astrology.PlayerSpotted();
            physics.PlayerSpotted();
            elements.PublicFollowPlayer();
            neuroscience.PlayerSpotted();
            geology.vision.PlayerSpotted = true;
        }


        //Jingle and stuff
        if(astrology == null && physics == null && elements == null && neuroscience == null && !AlrPlayedJingle)
        {
            AlrPlayedJingle = true;
            BattleTheme.gameObject.SetActive(false);
            RegularMusic.gameObject.SetActive(true);
            AudioP.PlayOneShot(Jingle);
        }
        



    }
}
