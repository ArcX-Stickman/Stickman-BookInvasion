using System.Collections;
using System.Collections.Generic;
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
    public bool NoMusicOnPauseForced;
    public GameObject Collectables;
    public bool Audio;
    public AudioSource AudioP;
    public AudioSource EmbryTheme;
    public AudioSource Hypothesis;
    public AudioSource BreakIN;
    public AudioSource BattleLow;
    public AudioSource BGLOW;
    public Movement PlayerManager;
    public GameObject Graphix;
    public GameObject PauseMusic;
    public Health StickHP;
    public AudioClip ButtonClick;
    public AudioSource Battle;
    public bool NoPauseSFX;
    public MaxFPS fpsManager;
    public LoadingSystem LoadingSystem;

    [Header("All Music Sources (except AudioP)")]
    public List<AudioSource> MusicSources;

    private void Start()
    {
        // Initial state: ensure we start in game mode
        isPaused = false;
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 1. Muting Logic (Handles gameplay states)
        HandleGameplayMuting();

        // 2. Input Handling
        // Only allow pausing if the player is alive and not in a special state
        bool canPauseInput = PlayerManager.StickHP.PlayerHP > 0 && !PlayerManager.Dead && !PlayerManager.isInPerfectDodge;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else if (canPauseInput && Time.timeScale == 1f)
            {
                Pause();
            }
        }

        // 3. Auto-Resume Safety (If player dies while menu is somehow open)
        if (isPaused && PlayerManager.Dead)
        {
            Resume();
        }
    }

    private void HandleGameplayMuting()
    {
        if (PlayerManager.isInPerfectDodge || PlayerManager.StickHP.PlayerHP <= 0)
        {
            if (EmbryTheme != null) EmbryTheme.mute = true;
            if (Hypothesis != null) Hypothesis.mute = true;
            if (BreakIN != null) BreakIN.mute = true;

            if (PlayerManager.StickHP.PlayerHP <= 0)
            {
                NoHUD = true;
            }
        }
    }

    public void Pause()
    {
        // Guard Clause: If we are already paused, don't run this logic again.
        if (isPaused) return;

        isPaused = true;

        // UI and Cursor Management
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (!NoHUD)
        {
            pauseMenuUI.SetActive(true);
            PauseMusic.gameObject.SetActive(true);

            if (PlayerManager.StickHP.PlayerHP > 0 && Audio)
            {
                ToggleMusic(true);
            }
        }

        // Forced mute logic
        if (PlayerManager.StickHP.PlayerHP > 0 && Audio && NoHUD && NoMusicOnPauseForced)
        {
            ToggleMusic(true);
        }

        PauseMenuAnim.SetTrigger("In");

        // Engine State
        if (!PlayerManager.isSitting)
        {
            Timemanager.SetActive(false);
            Time.fixedDeltaTime = 0f;
            Time.timeScale = 0f;
        }
    }

    public void Resume()
    {
        // Guard Clause: If we are already resumed, don't run this logic again.
        if (!isPaused)
        {
            // Failsafe: keep cursor locked during gameplay
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            return;
        }

        isPaused = false;

        // Reset Cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Reset UI Elements
        Collectables.gameObject.SetActive(false);
        OptionsMenu.gameObject.SetActive(false);

        if (PlayerManager.StickHP.PlayerHP > 0)
            AudioP.PlayOneShot(ButtonClick);

        // Physics/Cleanup
        StopAllCoroutines();
        Physics2D.simulationMode = SimulationMode2D.Script;
        Physics2D.Simulate(0.02f);
        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;

        EventSystem.current?.SetSelectedGameObject(null);
        Application.targetFrameRate = 60;

        // Animation and Time
        PauseMenuAnim.SetTrigger("Out");
        PauseMusic.gameObject.SetActive(false);
        Timemanager.SetActive(true);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        if (PlayerManager.StickHP.PlayerHP > 0 && Audio)
        {
            ToggleMusic(false);
        }

        // Garbage Collection and memory cleanup
        System.GC.Collect();
        Resources.UnloadUnusedAssets();
    }

    private void ToggleMusic(bool pause)
    {
        foreach (var music in MusicSources)
        {
            if (music != null && music.gameObject.activeSelf)
            {
                if (pause) music.Pause();
                else music.UnPause();
            }
        }
    }

    // --- Helper Methods & Button Hooks ---

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

            foreach (var music in MusicSources)
            {
                if (music != null)
                    music.gameObject.SetActive(true);
            }
        }
    }

    public void ThirtyFPS()
    {
        if (PlayerManager.StickHP.PlayerHP > 0) AudioP.PlayOneShot(ButtonClick);
        fpsManager.fps = 30;
    }

    public void SixtyFPS()
    {
        if (PlayerManager.StickHP.PlayerHP > 0) AudioP.PlayOneShot(ButtonClick);
        fpsManager.fps = 60;
    }

    public void OneTwentyFPS()
    {
        if (PlayerManager.StickHP.PlayerHP > 0) AudioP.PlayOneShot(ButtonClick);
        fpsManager.fps = 120;
    }

    public void Options()
    {
        AudioP.PlayOneShot(ButtonClick);
        OptionsMenu.gameObject.SetActive(!OptionsMenu.activeSelf);
    }

    public void Paper()
    {
        if (PlayerManager.StickHP.PlayerHP > 0) AudioP.PlayOneShot(ButtonClick);
        Collectables.gameObject.SetActive(!Collectables.activeSelf);
    }

    public void ResetGame()
    {
        if (PlayerManager.StickHP.PlayerHP > 0) AudioP.PlayOneShot(ButtonClick);
        SaveSystem.ResetGame();
        QuitGame();
    }

    public void LowGraphics()
    {
        if (PlayerManager.StickHP.PlayerHP > 0) AudioP.PlayOneShot(ButtonClick);
        Graphix.gameObject.SetActive(false);
    }

    public void HighGraphics()
    {
        if (PlayerManager.StickHP.PlayerHP > 0) AudioP.PlayOneShot(ButtonClick);
        Graphix.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        if (PlayerManager.StickHP.PlayerHP > 0) AudioP.PlayOneShot(ButtonClick);

        // Reset variables before scene load to prevent stuck states
        isPaused = false;
        Time.fixedDeltaTime = 0.02f;
        Time.timeScale = 1f;

        SceneManager.LoadScene("TitleScreen");



    }


    public void AudioOFF()
    {
       // hi
    }
}

