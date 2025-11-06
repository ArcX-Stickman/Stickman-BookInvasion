using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class EmbryTipsUI : MonoBehaviour
{
    public Image MainIMG;
    public Sprite[] TipIMG;
    public string[] TipText;
    public TextMeshProUGUI MainText;
    public int Value;
    public AudioSource AudioP;
    public AudioClip SFX;
    public TextMeshProUGUI R_Arrow;
    public TextMeshProUGUI L_Arrow;
    public PauseManager PauseManager;
    public GameObject Self;
    public bool IntroBattle;
    public Movement Player;
    public bool isGunTUT;
    // Start is called before the first frame update
    void Start()
    {
        Value = 0;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            PauseManager.NoHUD = true;
            PauseManager.Pause();

        }

        if (Value < TipIMG.Length - 1)
        {
            R_Arrow.color = Color.white;
          
        }
        if (Value > 0)
        {
            L_Arrow.color = Color.white;

           
        }
        MainIMG.sprite = TipIMG[Value];
        MainText.text = TipText[Value];
    }


    public void NextTip()
    {
        AudioP.PlayOneShot(SFX);
        if (Value < TipIMG.Length - 1)
        {
            R_Arrow.color = Color.white;
            Value++;
        }
        else
        {
            R_Arrow.color = Color.gray;
        }
    }

    public void ClosePopUp()
    {
        Self.gameObject.SetActive(false);
        PauseManager.Resume();
        PauseManager.NoHUD = false;
        if (IntroBattle)
        {
            Player.NeedsToViewDirecFirstIntro = true;
            Player.SaveProgress();
        }
        else if (isGunTUT)
        {
            Player.SeenGunDirections = true;
            Player.SaveProgress();
        } 

    }


    public void PreviousTip()
    {
        AudioP.PlayOneShot(SFX);

        if (Value > 0)
        {
            L_Arrow.color = Color.white;

            Value--;
        }
        else
        {
            L_Arrow.color = Color.gray;

        }

    }
}
