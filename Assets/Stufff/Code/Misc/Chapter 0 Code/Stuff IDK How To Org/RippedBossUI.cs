using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippedBossUI : MonoBehaviour
{
    public PauseManager PauseManager;
    public void PauseBreak()
    {
        PauseManager.NoHUD = false; 
        Destroy(gameObject);
    }
}
