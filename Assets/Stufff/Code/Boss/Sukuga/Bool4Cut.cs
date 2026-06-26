using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;
public class Bool4Cut : MonoBehaviour
{
    public VideoPlayer player;
    [SerializeField] private double timerThing;
    [SerializeField] private double VidDur;
    public bool CUT;
    public GameObject CUTS;

    void Update()
    {

        double VidDur = Math.Round(player.length, 1);

        timerThing = player.time;
        if (player.time + 0.1f >= VidDur)
        {
            CUTS.gameObject.SetActive(false);
            CUT = true;
            this.enabled = false;

        }

    }

}
