using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineWhip : MonoBehaviour
{
    public Biology Bio_Manager;
    public Movement PlayerMove;
    public Health PlayerHP;
    public Animator VineAnim;
    public GameObject VineGameObject;
    public bool TouchingStick;
    public float DMG;
    public AudioSource AudioP;
    public AudioClip WhipSFX;
    public AudioClip WhipMiss;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HideVine()
    {
        VineGameObject.SetActive(false);
    }

    public void WhipStickman()
    {
        if (TouchingStick)
        {
            AudioP.PlayOneShot(WhipSFX);
            PlayerHP.PlayerHP -= DMG;
            PlayerMove.StickAnim.SetTrigger("Hurt");
            Bio_Manager.ThrowStickman();
        }
        else
        {
            AudioP.PlayOneShot(WhipMiss);
        }
    }

    public void BiologyTouchBug()
    {
        Bio_Manager.BioAnim.SetTrigger("FalseVWhip");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TouchingStick = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Only set to false if the Player exits
        {
            TouchingStick = false;
        }
    }
}
