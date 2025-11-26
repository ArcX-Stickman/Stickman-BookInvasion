using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadingSystem : MonoBehaviour
{

    public GameObject LoadingScreen;
    public GameObject TitleHUD;
    public GameObject StickBoi;
    public MainMenu JustForASound;
    public GameObject Clouds;
    public GameObject Stickman;
    public GameObject Graphix;
    public Animator LilStickAnim;
    public bool ForTitle;
    public bool InGame;
    public PauseManager PauseManager;
    public GameObject Player;
    public GameObject Level1;
    public GameObject Level2;
    public void LoadGameScene()
    {
        if (ForTitle)
        {
            JustForASound.Click();
            TitleHUD.SetActive(false);
            StickBoi.gameObject.SetActive(true);
            Stickman.gameObject.SetActive(false);
            Clouds.SetActive(false);
            Graphix.gameObject.SetActive(false);
        }
        if (InGame)
        {
            PauseManager.NoPauseSFX = true;
            //Destroy(Player);
            PauseManager.AudioOFF();
            PauseManager.NoHUD = true;
            PauseManager.Pause();
            if(Level1 != null)
            {
                Destroy(Level1);

            }
            if (Level2 != null)
            {
                Destroy(Level2);
            }

        }

        LoadingScreen.SetActive(true);
       

        StartCoroutine(LoadScene());
    }

    public void QuitToTitle()
    {
        if (InGame && PauseManager != null)
        {
            PauseManager.NoPauseSFX = true;
            //Destroy(Player);
            PauseManager.AudioOFF();
            PauseManager.NoHUD = true;
            PauseManager.Pause();
            if (Level1 != null)
            {
                Destroy(Level1);

            }
            if (Level2 != null)
            {
                Destroy(Level2);
            }

        }

        LoadingScreen.SetActive(true);


        StartCoroutine(LoadTitle());
    }

    IEnumerator LoadTitle()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync("TitleScreen");
        loadOperation.allowSceneActivation = false;
        if (InGame && PauseManager != null)
        {
            PauseManager.Resume();
        }

        while (loadOperation.progress < 0.9f)
        {



            yield return null;
        }
        yield return new WaitForSeconds(1f);

        LilStickAnim.SetTrigger("Load");

        yield return new WaitForSeconds(1.5f);

        loadOperation.allowSceneActivation = true;


    }
    IEnumerator LoadScene()
    {
        PlayerData data = SaveSystem.LoadSave();
        
        string sceneToLoad = data?.SceneName ?? "Game";
        if(sceneToLoad == " ")
        {
            sceneToLoad = "Game";
        }

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        loadOperation.allowSceneActivation = false;

        if (InGame && PauseManager != null)
            PauseManager.Resume();

        while (loadOperation.progress < 0.9f)
            yield return null;

        yield return new WaitForSeconds(1f);
        LilStickAnim.SetTrigger("Load");
        yield return new WaitForSeconds(1.5f);

        loadOperation.allowSceneActivation = true;
    }



}
