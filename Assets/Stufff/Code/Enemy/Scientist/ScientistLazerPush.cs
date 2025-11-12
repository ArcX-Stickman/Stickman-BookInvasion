using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScientistLazerPush : MonoBehaviour
{
    public AudioSource AudioP;
    public AudioClip LazerUp;
    public LazerPole LazerManager;
    public Movement Stickman;

    public TutorialKeyInput TKI;

    public TextMeshProUGUI textMeshProUGUI;

    
    public void LazersGoingUp()
    {
        //Stickman.TimeManager.SloMO();
        AudioP.PlayOneShot(LazerUp);
        LazerManager.LazersUp = true;
        Stickman.TimeManager.SloMO();
        Stickman.TutTXT.gameObject.SetActive(true);

        TKI.gameObject.SetActive(true);
        TKI.InputS = KeyCode.C;
        textMeshProUGUI.text = "Slide[C]";
        
    }
}
