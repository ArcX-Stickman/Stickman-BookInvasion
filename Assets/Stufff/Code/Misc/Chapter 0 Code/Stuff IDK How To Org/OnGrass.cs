using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGrass : MonoBehaviour
{

    public Movement Stickman;
    public bool IsOnline;
    public StickOnlineManager StickOnlineManager;
    public bool Wood;


    public void Start()
    {
        if (IsOnline)
        {
            StickOnlineManager = FindObjectOfType<StickOnlineManager>();

        }
    }

    public void Update()
    {
        if (IsOnline)
        {
            StickOnlineManager = FindObjectOfType<StickOnlineManager>();

        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (IsOnline)
            {
                StickOnlineManager.WalkOnGrass = true;
            }
            else
            {
                if (!Wood)
                {
                    Stickman.WalkOnGrass = true;
                    Stickman.WalkOnWood = false;


                }
                else
                {
                    Stickman.WalkOnWood = true;
                    Stickman.WalkOnGrass = false;

                }

            }
        }
    }
}
