using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthKill : MonoBehaviour
{
    public Movement PlayerCaller;
    public Transform EnemyPos;
    public bool IsUI;
    public GameObject UI;
    public TimeManager TimeManager;
    private bool Done;
    public bool WantToUseVolume;
    public EnemyVolume EnemyVolume;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (WantToUseVolume)
        {
            if (gameObject.activeSelf && EnemyVolume.PlayerInVolume)
            {
                if (Input.GetKeyDown(KeyCode.F) && !Done)
                {
                    Done = true;
                    CallStealthFunc();
                }
                else
                {
                    Debug.Log("Nah");
                }
            }
        }
        else
        {
            if (gameObject.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.F) && !Done)
                {
                    Done = true;
                    CallStealthFunc();
                }
            }
        }


        
    }

    public void CallStealthFunc()
    {
        TimeManager.SloMO();
        if (IsUI)
        {
            Destroy(UI);
        }
        Debug.Log("Called");
        PlayerCaller.StartCoroutine(PlayerCaller.StealthKill(EnemyPos));
    }
}
