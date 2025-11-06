using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class StickOnlineHealth : NetworkBehaviour
{
    public NetworkVariable<float> PlayerHP = new NetworkVariable<float>();
    public bool LowHP;
    public bool InCombat;
    public GameObject UI;
    public bool BiologyBoss;
    public float MegaCharge;
    public bool CanSuper;
    public Slider HPslide;
    public float Multiplier;
    public float changerate;
    public float MaxHP = 100;
    public BattleV2 Music;
    public bool Testing;
    public StickOnlineManager StickManager;
    public AudioSource AudioP;
    public Image HP_BarIMG;




    public override void OnNetworkSpawn()
    {
        if (Testing)
        {
            PlayerHP.Value = 9999999999;
        }
        else
        {

            if (IsServer)
            {
                PlayerHP.Value = MaxHP;
            }

            if (IsOwner)
            {
                HPslide.value = PlayerHP.Value;
                HPslide.maxValue = MaxHP;
               

            }
            else
            {
                Destroy(UI);
                Debug.Log("Yo why u wanna be owner buddy");
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner)
        {
            UI.gameObject.SetActive(false);
            return;
        }

        GameObject sliderObj = GameObject.FindGameObjectWithTag("HPBAR");
        GameObject HP_BarIMG_Obj = GameObject.FindGameObjectWithTag("HPBARIMG");
        HPslide = sliderObj.GetComponent<Slider>();
        HP_BarIMG = HP_BarIMG_Obj.GetComponent<Image>();
        MegaCharge = 0f;
        PlayerHP.Value = MaxHP;
        LowHP = false;
    }





    // Update is called once per frame
    void Update()
    {
        if(!IsOwner)
        {
            return;
        }
        else
        {
            HPslide.value = PlayerHP.Value;
        }
        GameObject sliderObj = GameObject.FindGameObjectWithTag("HPBAR");
        GameObject HP_BarIMG_Obj = GameObject.FindGameObjectWithTag("HPBARIMG");
        HPslide = sliderObj.GetComponent<Slider>();
        HP_BarIMG = HP_BarIMG_Obj.GetComponent<Image>();
        if (Input.GetKey(KeyCode.F12) && Input.GetKeyDown(KeyCode.L))
        {
            Testing = true;
        }




        //Color for HUD

        if (PlayerHP.Value >= 60)
        {
            HP_BarIMG.color = Color.green;
        }
        else if (PlayerHP.Value < 40)
        {
            if (PlayerHP.Value < 25)
            {
                HP_BarIMG.color = Color.red;

            }
            else
            {
                HP_BarIMG.color = Color.yellow;

            }
        }











        if (Testing)
        {
            PlayerHP.Value = 9999999999;
        }


        if (MegaCharge >= 30)
        {
            MegaCharge = 30;
        }
        if (PlayerHP.Value > 100 && !Testing)
        {
            PlayerHP.Value = MaxHP;
            if (InCombat)
            {
                Music.NormalCombat = true;
            }
        }


        if (PlayerHP.Value < 0)
        {
            PlayerHP.Value = 0;
        }


        HPslide.value = PlayerHP.Value;

        MegaCharge = MegaCharge + changerate;



        if (PlayerHP.Value <= 30)
        {
            LowHP = true;
            if (InCombat && BiologyBoss == false)
            {
                Music.LowHp = true;
            }


        }


       


        if (PlayerHP.Value >= 30)
        {
            LowHP = false;

        }

        if (MegaCharge >= 30)
        {
            //CanSuper = true;
        }
    }


   



}

