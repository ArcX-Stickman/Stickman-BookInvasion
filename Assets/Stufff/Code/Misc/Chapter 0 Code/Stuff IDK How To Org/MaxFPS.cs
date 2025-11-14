using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxFPS : MonoBehaviour
{

    private float deltaTime;
    public int fps;
    public PauseManager pauseManager;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = fps;
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseManager == null)
        {
            pauseManager = FindAnyObjectByType<PauseManager>();
        }
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f; 
        float currentfps = 1.0f / deltaTime;
        if (currentfps < 9 && currentfps != 0)
        {
            pauseManager.Pause();
        }
        
        Application.targetFrameRate = fps;
        if(Input.GetKeyDown(KeyCode.M))
        {
            fps = 999;
        }

        
    }


    public void FPS()
    {

        

    }
}
