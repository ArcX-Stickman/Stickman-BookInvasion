using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUp_ON_OFF : MonoBehaviour
{


    public StickCloseUpSuk StickCloseUpSuk;
    public Animator Animator;

    public void Disable()
    {
        StickCloseUpSuk.enabled = false;
    }

    public void Enable()
    {
        StickCloseUpSuk.enabled = true;
    }
    public void HoldAnim(int i)
    {
        if(i <= 0)
        {
            Animator.SetBool("Hold",false);
        }
        else
        {
            Animator.SetBool("Hold", true);

        }

    }

}
