using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour
{

    public bool PlayerSpotted;
    public Health MusicTrigg;
    public bool Immediate;
    public bool UsedFor1stCut;
    public Movement Player;
    public bool PlayerExitedVision;
    // Start is called before the first frame update
    void Start()
    {
        PlayerSpotted = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (UsedFor1stCut)
            {
                if (Player.Cutscene_IntroFight)
                {
                    PlayerSpotted = true;

                }
                else
                {
                    Debug.Log("You gotta watch it first Boyy");
                }
            }
            else
            {
                PlayerSpotted = true;
                PlayerExitedVision = false;


            }
        }
        
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerExitedVision = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //hi
        if (Immediate)
        {
            PlayerSpotted = true;
        }
    }
}
