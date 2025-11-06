using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public static bool isPaused = false;
    public GameObject Timemanager;
    public GameObject OptionsMenu;

    public Animator PauseMenuAnim;

    // Audio
    public bool NoHUD;
    public GameObject Collectables;
    public bool Audio;
    public AudioSource AudioP;
    public AudioSource EmbryTheme;
    public AudioSource Hypothesis;
    public AudioSource BreakIN;
    public Movement PlayerManager;
    public GameObject Graphix;
    public GameObject PauseMusic;
    public Health StickHP;
    public AudioClip ButtonClick;
    public AudioSource Battle;
    public bool NoPauseSFX;
    public MaxFPS fpsManager;
    public LoadingSystem LoadingSystem;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
    }

    void Update()
    {
        if (!isPaused)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        if (PlayerManager.isInPerfectDodge)
        {
            EmbryTheme.mute = true;
            Hypothesis.mute = true;
            BreakIN.mute = true;

        }
        else
        {
            EmbryTheme.mute = false;
            Hypothesis.mute = false;
            BreakIN.mute = false;
        }

        if (PlayerManager.StickHP.PlayerHP <= 0)
        {
            EmbryTheme.mute = true;
            BreakIN.mute = true;
            Hypothesis.mute= true;
            NoHUD = true;

        }

        if (Input.GetKeyDown(KeyCode.Escape) && PlayerManager.StickHP.PlayerHP >= 0 && !PlayerManager.Dead && !PlayerManager.isInPerfectDodge)
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
        if(isPaused && PlayerManager.Dead)
        {
            Resume();
        }
    }

    public void RestartCheckpoint()
    {
        LoadingSystem.LoadGameScene();
    }

    public void AudioOn()
    {
        if (PlayerManager.StickHP.PlayerHP > 0)
            AudioP.PlayOneShot(ButtonClick);

        Audio = true;

        if (PlayerManager.StickHP.PlayerHP > 0)
        {
            if (PlayerManager.BreakIn == false && Audio)
            {
                if (Battle.gameObject.activeSelf)
                    Battle.Play();
                else
                    Hypothesis.gameObject.SetActive(true);

                BreakIN.gameObject.SetActive(false);
            }
            else if (PlayerManager.BreakIn == true && Audio)
            {
                Hypothesis.gameObject.SetActive(false);
                BreakIN.gameObject.SetActive(true);
            }

            if (EmbryTheme != null && EmbryTheme.gameObject.activeSelf && Audio)
            {
                EmbryTheme.gameObject.SetActive(true);
            }
        }
    }

    public void SixtyFPS()
    {
        if (PlayerManager.StickHP.PlayerHP > 0)
            AudioP.PlayOneShot(ButtonClick);

        fpsManager.fps = 30;
    }

    public void OneTwentyFPS()
    {
        if (PlayerManager.StickHP.PlayerHP > 0)
            AudioP.PlayOneShot(ButtonClick);

        fpsManager.fps = 60;
    }

    public void AudioOFF()
    {
        // Reserved for future mute implementation
    }

    public void Resume()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Collectables.gameObject.SetActive(false);

        if (PlayerManager.StickHP.PlayerHP > 0)
            AudioP.PlayOneShot(ButtonClick);

        OptionsMenu.gameObject.SetActive(false);

        StopAllCoroutines();
        Physics2D.simulationMode = SimulationMode2D.Script;
        Physics2D.Simulate(0.02f);
        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;

        EventSystem.current?.SetSelectedGameObject(null);
        Application.targetFrameRate = 60;
        System.GC.Collect();
        Resources.UnloadUnusedAssets();

        PauseMenuAnim.SetTrigger("Out");
        PauseMusic.gameObject.SetActive(false);
        Timemanager.SetActive(true);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        isPaused = false;

        if (PlayerManager.StickHP.PlayerHP > 0)
        {
            if (PlayerManager.BreakIn == false && Audio)
            {
                if (Battle.gameObject.activeSelf)
                    Battle.UnPause();
                else
                    Hypothesis.UnPause();

                BreakIN.gameObject.SetActive(false);
            }
            else if (PlayerManager.BreakIn == true && Audio)
            {
                Hypothesis.gameObject.SetActive(false);
                BreakIN.UnPause();
            }

            if (EmbryTheme != null && PlayerManager.InEmbryBoss && EmbryTheme.gameObject.activeSelf && Audio)
                EmbryTheme.UnPause();
        }
    }

    public void Options()
    {
        AudioP.PlayOneShot(ButtonClick);

        OptionsMenu.gameObject.SetActive(!OptionsMenu.activeSelf);
    }

    public void Paper()
    {
        if (PlayerManager.StickHP.PlayerHP > 0)
            AudioP.PlayOneShot(ButtonClick);

        Collectables.gameObject.SetActive(!Collectables.activeSelf);
    }

    public void ResetGame()
    {
        if (PlayerManager.StickHP.PlayerHP > 0)
            AudioP.PlayOneShot(ButtonClick);

        SaveSystem.ResetGame();
        QuitGame();
    }

    public void LowGraphics()
    {
        if (PlayerManager.StickHP.PlayerHP > 0)
            AudioP.PlayOneShot(ButtonClick);

        Graphix.gameObject.SetActive(false);
    }

    public void HighGraphics()
    {
        if (PlayerManager.StickHP.PlayerHP > 0)
            AudioP.PlayOneShot(ButtonClick);

        Graphix.gameObject.SetActive(true);
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;


        if (!NoHUD)
        {
            pauseMenuUI.SetActive(true);
            PauseMusic.gameObject.SetActive(true);
        }

        Time.fixedDeltaTime = 0f;
        PauseMenuAnim.SetTrigger("In");

        Timemanager.SetActive(false);

        if (PlayerManager.StickHP.PlayerHP > 0)
        {
            if (EmbryTheme != null && EmbryTheme.gameObject.activeSelf && Audio && !NoHUD)
                EmbryTheme.Pause();

            if (Battle.gameObject.activeSelf && !NoHUD && Audio)
                Battle.Pause();

            Hypothesis.Pause();
            BreakIN.Pause();
        }

        Time.timeScale = 0f;
        isPaused = true;
    }

    public void QuitGame()
    {
        if (PlayerManager.StickHP.PlayerHP > 0)
            AudioP.PlayOneShot(ButtonClick);

        SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
        Time.fixedDeltaTime = 0.02f;
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScreen");
    }
}
