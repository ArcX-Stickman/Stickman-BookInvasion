using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Controls_Tutorial : MonoBehaviour
{
    public Animator animator;
    public Movement Stick;
    public TextMeshProUGUI Text;
    public GameObject Paper;
    public AudioSource AudioP;
    public AudioClip PaperPullUp;
    public AudioClip PaperThrow;
    private string WASD = "[WASD] TO MOVE";
    private string Jump = "[SPACE] JUMP";
    private string Backflip = "[Q] FOLLOW JUMP WITH A BACKFLIP TO REACH HIGH LEDGES";
    private string Momentum = "Running without breaks builds momentum letting you jump higher. Try running back and forth to gain more speed quicker";
    public string Dash = "[Shift] Dashing will allow you to travel in the air for a longer period of time and To Reposition in Combat By Dashing Over An Enemy";
    public GameObject BackFlipIMG;
    public GameObject MomentumIMG;
    public GameObject DashIMG;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && Paper.gameObject.activeSelf)
        {
            Close();
        }
    }
    public void PullUp()
    {
        AudioP.PlayOneShot(PaperPullUp);
        Paper.gameObject.SetActive(true);
    }

    public void Close()
    {
        AudioP.PlayOneShot(PaperThrow);
        Paper.gameObject.SetActive(false);
        BackFlipIMG.gameObject.SetActive(false);
        MomentumIMG.gameObject.SetActive(false);


    }

    public void BackFlip_Text()
    {
        Text.text = Backflip;
        BackFlipIMG.gameObject.SetActive(true);
    }
    //
    public void Momentum_Text()
    {
        Text.text = Momentum;
        MomentumIMG.gameObject.SetActive(true);

    }
    public void Dash_Text()
    {
        Text.text = Dash;
        DashIMG.gameObject.SetActive(true);
    }
}
