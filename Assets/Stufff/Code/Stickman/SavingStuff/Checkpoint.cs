using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{

    public int CheckpointNumber;
    public Movement PlayerManager;
    public GameObject SaveIcon;
    public bool OnlyDebuggingPurp;
    private bool AlrDid;
    public bool WantSpawnObject;
    public GameObject Obj;
    public bool AlrDoneit;
    public AudioSource AudioP;
    public AudioClip SFX;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!AlrDid && WantSpawnObject)
            {
                if(AudioP != null && SFX != null)
                {
                    AudioP.PlayOneShot(SFX);
                }
                Obj.gameObject.SetActive(true);
                AlrDid = true;

            }
            PlayerManager.SceneName = SceneManager.GetActiveScene().name;
            PlayerManager.Checkpoint = CheckpointNumber;
            if (!AlrDoneit && PlayerManager.StickHP.PlayerHP > 0 && !OnlyDebuggingPurp)
            {
                SaveIcon.gameObject.SetActive(true);
                AlrDoneit = true;
                PlayerManager.StickHP.MegaCharge += 5;
                PlayerManager.SaveProgress();


            }
            else if(OnlyDebuggingPurp && PlayerManager.StickHP.Testing)
            {
                SaveIcon.gameObject.SetActive(true);
                AlrDoneit = true;
                PlayerManager.StickHP.MegaCharge += 5;
                PlayerManager.SaveProgress();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerManager.Checkpoint = CheckpointNumber;
        if (!AlrDoneit && PlayerManager.StickHP.PlayerHP > 0)
        {
            SaveIcon.gameObject.SetActive(true);
            AlrDoneit = true;
            PlayerManager.SaveProgress();


        }
    }
}
