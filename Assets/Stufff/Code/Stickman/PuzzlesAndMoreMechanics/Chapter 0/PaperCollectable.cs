using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PaperCollectable : MonoBehaviour
{
    
    public GameObject UI;
    public TextMeshProUGUI TextUI;
    public GameObject BackflipText;
    public GameObject InvasionText;
    public bool isUI;
    public bool isBackflip;
    public PauseManager PauseThing;
    public AudioClip PaperSound;
    public AudioSource AudioP;
    public AudioClip Click;
    public Movement Stickman;
  


    private void Start()
    {
    }

    public void Backflip()
    {
        BackflipText.gameObject.SetActive(true);
        UI.gameObject.SetActive(true);
        TextUI.text = "Backflip to earn extra altitude";
        if (!isUI)
        {
            Stickman.BackflipCollected = true;

            Stickman.SaveProgress();
            AudioP.PlayOneShot(PaperSound);
            Destroy(gameObject);

        }
        else
        {
            AudioP.PlayOneShot(Click);

            PauseThing.Resume();
        }
    }


    public void NoMoreSticks()
    {
        BackflipText.gameObject.SetActive(true);
        UI.gameObject.SetActive(true);
        TextUI.text = "After This Invasion Is Over No Stickman Will Be Left";
        if (!isUI)
        {
            Stickman.NoMoreCollected = true;

            Stickman.SaveProgress();
            AudioP.PlayOneShot(PaperSound);
            Destroy(gameObject);

        }
        else
        {
            AudioP.PlayOneShot(Click);

            PauseThing.Resume();
        }
    }

    public void AboutInvasion()
    {
        InvasionText.gameObject.SetActive(true);
        UI.gameObject.SetActive(true);
        TextUI.text = "Ever Since Planet Page Got Destroyed We Had To Resort To Finding Another Planet For Our Race To Live On, And Then We Found Planet Earth.";
        if (!isUI)
        {
            Stickman.InfoCollected = true;

            Stickman.SaveProgress();
            AudioP.PlayOneShot(PaperSound);
            Destroy(gameObject);

        }
        else
        {
            AudioP.PlayOneShot(Click);

            PauseThing.Resume();
        }
    }
}
