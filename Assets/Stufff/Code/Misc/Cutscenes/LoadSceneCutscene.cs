using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System;

public class LoadSceneCutscene : MonoBehaviour
{
    public VideoPlayer player;
    public GameObject LoadedObject;
    [SerializeField] private double timerThing;
    [SerializeField] private double VidDur;
    public GameObject CUT;

    public bool PreLoad;
    private AsyncOperation loadOperation;
    public bool DontDecSelf;

    public string sceneToLoad;
    private void Start()
    {
        loadOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        loadOperation.allowSceneActivation = false;
    }

    void Update()
    {
        if(loadOperation == null)
        {
            loadOperation = SceneManager.LoadSceneAsync(sceneToLoad);
            loadOperation.allowSceneActivation = false;
        }

        double VidDur = Math.Round(player.length, 1);

        timerThing = player.time;
        if (player.time + 0.1f >= VidDur)
        {
            loadOperation.allowSceneActivation = true;

            if (!DontDecSelf)
            {
                CUT.gameObject.SetActive(false);

            }
            this.enabled = false;

        }

    }
}
