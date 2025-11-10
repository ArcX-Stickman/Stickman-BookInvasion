using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip SFX;
    public PauseManager Pause;
    private bool Alrdid;
    public AudioSource BG;
    void Update()
    {
        if (gameObject.activeSelf && !Alrdid)
        {
            if(BG != null)
            {
                if(!BG.gameObject.activeSelf || !BG.isPlaying || BG.volume == 0)
                {
                    BG.gameObject.SetActive(true);
                    BG.Play();
                    BG.volume = 1;
                }
            }
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
