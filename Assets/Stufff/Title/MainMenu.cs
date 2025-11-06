using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator Stickman;

    public bool CanTransition;
    public bool Story;
    public GameObject Selection1;
    public GameObject Selection2;
    public JustAShootSFX StickManager;
    public bool DesktopGo;
    public GameObject ResetButton;

    private void Start()
    {
        Selection2.gameObject.SetActive(false);
        CanTransition = false;
    }

    private void Update()
    {
        if (CanTransition && Story)
        {
            SceneManager.LoadScene(1);
        }

        if(CanTransition && DesktopGo)
        {
            Application.Quit();
        }
    }

    public void PlayGame()
    {
        StickManager.Fadeactive = true;
        Story = true;
        Click();
        
        
    }


    public void Survival()
    {
        StickManager.Fadeactive = true;
        
        Click();
        SceneManager.LoadSceneAsync("Survival");

    }

    public void Click()
    {
        Stickman.SetTrigger("Shoot");
    }

    public void StartButton()
    {
        Selection1.gameObject.SetActive(false);
        Selection2.gameObject.SetActive(true);
        Click();
    }

    public void Back()
    {
        Story = false;
        Selection1.gameObject.SetActive(true);
        Selection2.gameObject.SetActive(false);
        Click();
    }

    public void Reset()
    {
        SaveSystem.ResetGame();
        Destroy(ResetButton);
        Stickman.SetTrigger("Shoot");

    }

    public void Online()
    {
        SceneManager.LoadSceneAsync("OnlineGame");

    }
    public void Desktop()
    {
        Click();
        StickManager.Fadeactive = true;
        DesktopGo = true;

        if (CanTransition)
        {
            Application.Quit();

        }
            
        
    }

}
