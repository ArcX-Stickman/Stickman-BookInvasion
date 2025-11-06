using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip SFX;
    public PauseManager Pause;

    void Update()
    {
        if (gameObject.activeSelf)
        {
            Pause.NoHUD = true;
            Pause.Pause();
        }
    }

    public void Close()
    {
        audioSource.PlayOneShot(SFX);
        Destroy(gameObject);
        Pause.NoHUD = false;
        Pause.Resume();


    }
}
