using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;
public class ActivateOnCutSceneEnd : MonoBehaviour
{
    public VideoPlayer player;
    public GameObject LoadedObject;
    [SerializeField] private double timerThing;
    [SerializeField] private double VidDur;
    void Update()
    {

        double VidDur = Math.Round(player.length, 1);

        timerThing = player.time;
        if (player.time + 0.1f >= VidDur)
        {
            LoadedObject.gameObject.SetActive(true);

            this.gameObject.SetActive(false);
        }

      
    }
}
