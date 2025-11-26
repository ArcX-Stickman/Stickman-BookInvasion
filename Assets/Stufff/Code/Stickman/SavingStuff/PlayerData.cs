using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    //Progression
    public bool HasGun;
    public int Checkpoint;
    public float[] PlayerPOS;
    public bool HasBrokeOutOfRope;
    public string SceneName;
    // Bosses
    public bool HasKilledEmbry;
    public bool HasFinishedEncounterAfterLab;
    public bool HasKilledEcology;

    // Abilities
    public bool UnlockedBackflip;
    public bool UnlockedSlide;

    public bool UnlockedDash;
    

    //Cutscenes And Stuff
    public bool Cutscene_IntroFight;
    public bool NeedsToViewDirecFirstIntro;
    public bool HasFullyWatchedFirst;
    public bool SeenGunDirections;


    //Collectables
    public bool Backflip;
    public bool InfoCollect;
    public bool NoMoreCollect;

    //Stuff For PerkTree
    public int XP_Orbs;
    public PlayerData (Movement PlayerManager)
    {
        // Player Data and Sum Progression
        SceneName = PlayerManager.SceneName;
        HasGun = PlayerManager.HasGun;
        Checkpoint = PlayerManager.Checkpoint;
        PlayerPOS =  new float[3];
        PlayerPOS[0] = PlayerManager.transform.position.x;
        PlayerPOS[1] = PlayerManager.transform.position.y;
        PlayerPOS[2] = PlayerManager.transform.position.z;
        HasBrokeOutOfRope = PlayerManager.HasBrokeOutOfRope;

        // Player Ability
        UnlockedBackflip = PlayerManager.UnlockedBackflip;
        UnlockedDash = PlayerManager.UnlockedDash;
        UnlockedSlide = PlayerManager.UnlockedSlide;

        // Collectables
        Backflip = PlayerManager.BackflipCollected;
        InfoCollect = PlayerManager.InfoCollected;
        NoMoreCollect = PlayerManager.NoMoreCollected;

        //Cutscenes/Directions
        Cutscene_IntroFight = PlayerManager.Cutscene_IntroFight;
        NeedsToViewDirecFirstIntro = PlayerManager.NeedsToViewDirecFirstIntro;
        HasFullyWatchedFirst = PlayerManager.HasFullyWatchedFirst;
        SeenGunDirections = PlayerManager.SeenGunDirections;

        // Bosses N Encounters
        HasKilledEmbry = PlayerManager.HasKilledEmbry;
        HasFinishedEncounterAfterLab = PlayerManager.FinshedEncounterAfterLab;
        HasKilledEcology = PlayerManager.HasKilledEcology;

        //PerkTree
        XP_Orbs = PlayerManager.XP_Orbs;

    }








    // We finally making a save bout time

}
