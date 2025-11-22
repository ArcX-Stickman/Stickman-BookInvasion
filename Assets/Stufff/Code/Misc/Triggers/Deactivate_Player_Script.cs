using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deactivate_Player_Script : MonoBehaviour
{
    public Movement Stick;
    public StickMoveGlitch StickMoveGlitch;
    
    void Update()
    {
        if (gameObject.activeSelf)
        {
            Stick.ResetAllAnimationStates();
            Stick.enabled = false;
            Stick.enabled = false;
            Stick.rb.velocity = Vector3.zero;
            StickMoveGlitch.enabled = false;
            Stick.GrassBlades.gameObject.SetActive(false);
            Stick.FootP.mute = true;

        }
    }
}
