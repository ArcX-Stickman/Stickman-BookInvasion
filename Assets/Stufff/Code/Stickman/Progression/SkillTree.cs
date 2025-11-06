using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    [Header("Skill Tree Perks")]
    public GameObject DashPerk;
    public GameObject PocoFuerte;
    public GameObject JumpingIsFree;
    public GameObject TuffCombo;
    public GameObject SlideSumMeds;
    public GameObject BulletBarrage;
    public GameObject PunchNKick;
    [Header("Icons Images")]
    public Sprite PerkEmpty;
    [Header("Booleans For Skill Tree")]
    public bool HasDashPerk;
    public bool HasPocoFuerte;
    public bool HasJumpingIsFree;
    public bool HasTuffCombo;
    public bool HasSlideSumMeds;
    public bool HasBulletBarrage;
    public bool HasPunchNKick;
    [Header("Player Current Progression")]
    public int CurrentLevel;
    public int OrbCount;
    public Movement Stickman;
    public TextMeshProUGUI OrbCountDisplay;
    [Header("Perk Icon Sprite Renders")]
    public Image DashPerkSpriteR;
    public Image PocoFuerteSpriteR;
    public Image JumpingIsFreeSpriteR;
    public Image TuffSpriteR;
    public Image SlideSumMedSpriteR;
    public Image BulletBarrageSpriteR;
    public Image PunchNKickSpriteR;
    [Header("Perk Icon Sprites")]
    public Sprite PocoFuerteSprite;
    public Sprite JumpingIsFreeSprite;
    public Sprite TuffSprite;
    public Sprite SlideSumMedSprite;
    public Sprite BulletBarrageSprite;
    public Sprite PunchNKickSprite;
    [Header("Jus sum stuff")]
    public AudioSource AudioP;
    public AudioClip ClickSFX;


    void Start()
    {
       
        DashPerkSpriteR = DashPerk.GetComponent<Image>();
        PocoFuerteSpriteR = PocoFuerte.GetComponent<Image>();
        JumpingIsFreeSpriteR = JumpingIsFree.GetComponent<Image>();
        SlideSumMedSpriteR = SlideSumMeds.GetComponent<Image>();
        BulletBarrageSpriteR = BulletBarrage.GetComponent<Image>();
        PunchNKickSpriteR = PunchNKick.GetComponent<Image>();
        TuffSpriteR = TuffCombo.GetComponent<Image>();
    }
    void Update()
    {
       ManageProgressionIcons();
        //OrbCount = Stickman.XP_Orbs; // add later
    }

    // Perk Buttons

    public void PunchNKickPerk()
    {
        AudioP.PlayOneShot(ClickSFX);
        HasPunchNKick = true;
        PunchNKickSpriteR.color  = new Color32(174, 174, 174, 255);
        Button button = PunchNKick.GetComponent<Button>();
        button.enabled = false;
    }
    public void PocoFuertePerk()
    {
        AudioP.PlayOneShot(ClickSFX);
        PocoFuerteSpriteR.color = new Color32(174, 174, 174, 255);
        HasPocoFuerte = true;
        Button button = PocoFuerte.GetComponent<Button>();
        button.enabled = false;
    }
    public void ExtendedDash()
    {
        AudioP.PlayOneShot(ClickSFX);
        HasDashPerk = true;
        Button button = DashPerk.GetComponent<Button>();
        DashPerkSpriteR.color = new Color32(174, 174, 174, 255);
        button.enabled = false;
    }
    public void TuffComboPerk()
    {
        AudioP.PlayOneShot(ClickSFX);
        HasTuffCombo = true;
        TuffSpriteR.color = new Color32(174, 174, 174, 255);
        Button button = TuffCombo.GetComponent<Button>();
        button.enabled = false;
    }


    public void ManageProgressionIcons()
    {
        //Shows if The skill is a currently unlockable
        // Skills that need A Prerequiste idk how to spell dat mb
        // Slide sum meds - Poco Fuerte
        // Jumps Free - Extended Dash
        // Tuff Combo - PunchNKick
        // Bullet Barrage - Tuff Combo - PunchNKick
        if (!HasDashPerk)
        {
            JumpingIsFreeSpriteR.sprite = PerkEmpty;
        }
        else
        {
            JumpingIsFreeSpriteR.sprite = JumpingIsFreeSprite;
        }
        
        if(!HasPocoFuerte)
        {
            SlideSumMedSpriteR.sprite = PerkEmpty;
        }
        else
        {
            SlideSumMedSpriteR.sprite = SlideSumMedSprite;
        }

        if (!HasPunchNKick)
        {
            TuffSpriteR.sprite = PerkEmpty;
        }
        else
        {
            TuffSpriteR.sprite = TuffSprite;
        }

        if (!HasTuffCombo)
        {
            BulletBarrageSpriteR.sprite = PerkEmpty;
        }
        else
        {
            BulletBarrageSpriteR.sprite = BulletBarrageSprite;
        }
    }
}
