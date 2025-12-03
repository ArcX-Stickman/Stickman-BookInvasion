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

    public bool IsUsedAsDistanceBasedTrigger;
    public float Distance;
    public float MinDIS;
    public Transform Stick;

    void Start()
    {
        PlayerSpotted = false;
    }

    void Update()
    {
        if (IsUsedAsDistanceBasedTrigger)
        {
            Distance = Vector2.Distance(transform.position, Stick.position);
            PlayerSpotted = Distance <= MinDIS;
            return;
        }


        if (Immediate)
        {
            PlayerSpotted = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsUsedAsDistanceBasedTrigger) return;

        if (collision.CompareTag("Player") && !IsUsedAsDistanceBasedTrigger)
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
        if (IsUsedAsDistanceBasedTrigger) return;

        if (collision.CompareTag("Player") && !IsUsedAsDistanceBasedTrigger)
        {
            PlayerExitedVision = true;
        }
    }
}
