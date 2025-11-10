using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnUI_ForTraps : MonoBehaviour
{
    public Movement Stick;
    public Vector3 SafePosition;
    // This freezes the player while the fade happens so they dont keep dying
    public void HelpPlayer()
    {
       
        Stick.enabled = false;
    }


    public void ReEnablePlayerScript()
    {
  
        Stick.enabled = true;
    }

    public void SpawnPlayer()
    {
        
        if(SafePosition != Vector3.zero)
        {
            Stick.transform.position = SafePosition;
        }
    }




}
