using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GunItem : MonoBehaviour
{


    public SpriteRenderer Pistol;
    public Light2D Light;
    public GameObject Hud;
    public TextMeshProUGUI HUD_Text;
    public Movement PlayerManger;
    private bool AlrDid;
    public PauseManager PauseManager;
    public Animator HUDANim;
    public GameObject GunPOP_UP;
    public bool NoPopUP;
    public Stick_TriggerEvent StickTrigger;
    public bool isDream;
    private Light2D L2d;
    

    public void Start()
    {
        if (PlayerManger.HasGun && PlayerManger.Checkpoint != 551)
        {
            gameObject.SetActive(false);

        }
    }

    private void Update()
    {
        if(L2d == null)
        {
            L2d = GetComponent<Light2D>();
        }

        if(!PlayerManger.HasGun && StickTrigger.Trigger)
        {
            PlayerManger.HasGun = true;
            PlayerManger.SaveProgress();
            Pistol.enabled = false;
            L2d.enabled = false;

        }
        if (GunPOP_UP != null && StickTrigger.Trigger2 )
        {
            GunPOP_UP.gameObject.SetActive(true);

        }
        if (isDream && PlayerManger.HasGun)
        {
            PlayerManger.StickAnim.SetTrigger("CinematicDone");
            PlayerManger.HasGun = true;
            PlayerManger.SaveProgress();
            Destroy(gameObject);

        }
        if (PlayerManger.HasGun || StickTrigger.Trigger)
        {
            L2d.enabled = false;

            Pistol.enabled = false;

        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(!AlrDid)
            {
                PlayerManger.Anim_Locked = true;
                AlrDid = true;
                PlayerManger.StickAnim.CrossFade("Item",0,0);
                
            }
            
        }
    }



  
}
