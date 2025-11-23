using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_MusicGlitch : MonoBehaviour
{
    public GameObject BG_music;
    public GameObject[] IfTheseObjectsOffThenTurnOnBG;

    private bool hasGlitched = false; 

    void Update()
    {
        if (!hasGlitched && AllObjectsOff())
        {
            BG_music.SetActive(true);
            hasGlitched = true; 
        }
    }

    bool AllObjectsOff()
    {
        foreach (GameObject obj in IfTheseObjectsOffThenTurnOnBG)
        {
            if (obj.activeSelf)
            {
                return false;
            }
        }
        return true;
    }
}
