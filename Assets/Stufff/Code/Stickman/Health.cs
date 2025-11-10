using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float PlayerHP;
    public bool LowHP;
    public GameObject LowHpFlashUI;
    public bool InCombat;
    public bool BiologyBoss;
    public float MegaCharge;
    public bool CanSuper;
    public Slider HPslide;
    public float Multiplier;
    public float changerate;
    public float MaxHP = 100;
    public BattleV2 Music;
    public bool Testing;
    public Movement StickManager;
    public TextMeshProUGUI HealCountText;
    public int HealCount;
    public AudioSource AudioP;
    public AudioClip HealSFX;
    public GameObject HealSpriteOverlay;
    public Image HP_BarIMG;
    public GameObject LowHP_Graphix;


    private float DelayWhenDepletedCD = 0.1f;
    private float LastDelay;
    private bool CanChargeMegaCharge;
    private bool UsedToMax;
    private bool CanDoStuff;

    private float UsedToMaxCD = 0.12f;
    private float LastUSedToMax;
    private float OG_MEGA_CHARGE_RATE;
    private float TEMP_MEGA_CHARGE_RATE;

    //Key For TUT
    public TutorialKeyInput TKI;
    public TextMeshProUGUI TextTUT_TXT;
    public string Direction;
    private float HealTip_CD = 2.5f;
    private float LastHealTip = 0;

    // Start is called before the first frame update
    void Start()
    {
        CanChargeMegaCharge = true;
        if (Testing)
        {
            PlayerHP = 9999999999;
        }
        OG_MEGA_CHARGE_RATE = changerate;
        HealSpriteOverlay.gameObject.SetActive(false);

        StartCoroutine(WaitForABit());
        HealCount = 3;
        MegaCharge = 0f;
        PlayerHP = MaxHP;
        LowHP = false;
        LowHpFlashUI.gameObject.SetActive(false);
    }


    private IEnumerator WaitForABit()
    {
        yield return new WaitForSecondsRealtime(2);
        CanDoStuff = true;
        yield return null;
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.F12) && Input.GetKeyDown(KeyCode.L))
        {
            Testing = true;
        }


        HealCountText.text = HealCount + ""; // unity and VS make me mad why cant i do without the string grrrrr >:(


        //Color for HUD
        if(LowHP_Graphix != null)
        {
            if (PlayerHP >= 60 || PlayerHP == 0)
            {
                HP_BarIMG.color = Color.green;
                LowHP_Graphix.gameObject.SetActive(false);

            }
            else if (PlayerHP < 60)
            {
                if(Time.time >= LastHealTip + HealTip_CD && HealCount > 0)
                {
                    LastHealTip = Time.time;
                    TKI.gameObject.SetActive(true);
                    TextTUT_TXT.text = Direction;
                    TKI.InputS = KeyCode.E;
                }
                LowHP_Graphix.gameObject.SetActive(true);
                if (PlayerHP <= 25)
                {
                    HP_BarIMG.color = Color.red;

                }
                else
                {
                    HP_BarIMG.color = Color.yellow;

                }
            }
        }
       
        










        if (Testing)
        {
            PlayerHP = 9999999999;
            MegaCharge = 9999999999;
            StickManager.MaxDodge = 1;
        }


        if(MegaCharge >= 30)
        {
            MegaCharge = 30;
        }
        if (PlayerHP > 100 && !Testing)
        {
            PlayerHP = MaxHP;
            if (InCombat)
            {
                Music.NormalCombat = true;
            }
        }
        if(MegaCharge <= 0)
        {
            MegaCharge = 0;
            
        }
        if (MegaCharge <= 0 && CanDoStuff)
        {
            if (!IsDelayingMegaCharge && Time.time >= UsedToMaxCD + LastUSedToMax)
            {
                LastUSedToMax = Time.time;
                UsedToMax = true;
                LastDelay = Time.time; 
                StartCoroutine(DelayinMegaCharge());
            }
        }


        if (PlayerHP < 0)
        {
            PlayerHP = 0;
        }


        HPslide.value = PlayerHP;
        if (CanChargeMegaCharge && Time.deltaTime > 0)
        {
            MegaCharge += changerate * Time.deltaTime;


        }



        if (PlayerHP <= 30)
        {
            LowHpFlashUI.gameObject.SetActive(true);
            LowHP = true;
            if (InCombat && BiologyBoss == false)
            {
               Music.LowHp = true;
            }
            
            
        }

        if (Input.GetMouseButtonDown(2))
        {
            if(PlayerHP <= 99 && HealCount > 0)
            {

                StickManager.StickAnim.SetTrigger("Heal");

            }
            if (Testing)
            {
                StickManager.PerfectDodgeCount = StickManager.MaxDodge;
            }
            
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            if (PlayerHP <= 99 && HealCount > 0 && !StickManager.isInPerfectDodge)
            {

                StickManager.StickAnim.SetTrigger("Heal");


            }

        }

        if(MegaCharge <= 1.9f)
        {
           TEMP_MEGA_CHARGE_RATE = OG_MEGA_CHARGE_RATE + 0.5f;
           changerate = TEMP_MEGA_CHARGE_RATE;
        }
        else
       {
            changerate = OG_MEGA_CHARGE_RATE;
        }

        if (PlayerHP >= 30)
        {
            LowHpFlashUI.gameObject.SetActive(false);
            LowHP = false;

        }

        if(MegaCharge >= 30)
        {
            //CanSuper = true;
        }
    }

    private bool IsDelayingMegaCharge = false;

    public void Heal()
    {
        HealSpriteOverlay.gameObject.SetActive(true);

        AudioP.PlayOneShot(HealSFX);
        HealCount -= 1;
        PlayerHP += 50;
    }

    public IEnumerator DelayinMegaCharge()
    {
        if (IsDelayingMegaCharge )
            yield break;

        IsDelayingMegaCharge = true;
        Debug.Log("Stamina DelayCalled");

        CanChargeMegaCharge = false;
        if(MegaCharge >= 1.9f)
        {
            yield return new WaitForSecondsRealtime(0.9f);

        }
        else
        {
            yield return new WaitForSecondsRealtime(1.3f);

        }


        CanChargeMegaCharge = true;
        UsedToMax = false;
        IsDelayingMegaCharge = false;
    }


}
