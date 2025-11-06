using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperCollect : MonoBehaviour
{
    public bool isBACKF;
    public bool isMomentum;
    public bool isDash;
    public Controls_Tutorial ControlManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SwitchText();
        }
    }
    public void SwitchText()
    {
        ControlManager.PullUp();
        if (isBACKF)
        {
            ControlManager.BackFlip_Text();
        }
        if (isDash)
        {
            ControlManager.Dash_Text();
        }
        else if(isMomentum) 
        {
            ControlManager.Momentum_Text();
        }
        Destroy(gameObject);

    }
}
