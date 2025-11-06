using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public Health PlayerHealthCode;
    public AudioSource AudioPlayer;
    public AudioClip BattleNOR;
    public AudioClip BattleLOWHP;

    public bool isPlayingBattleNOR = false;
    public bool isPlayingBattleLOWHP = false;

    // Start is called before the first frame update
    void Start()
    {
        PlayerHealthCode.InCombat = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is in combat
        if (PlayerHealthCode.InCombat)
        {
            // If HP is low, play the low HP sound if it's not already playing
            if (PlayerHealthCode.LowHP && !isPlayingBattleLOWHP)
            {
                AudioPlayer.Stop();
                AudioPlayer.PlayOneShot(BattleLOWHP);
                isPlayingBattleLOWHP = true;
                isPlayingBattleNOR = false; // Ensure BattleNOR is not playing
            }
            // If HP is not low and BattleNOR is not playing, play the BattleNOR sound
            else if (!PlayerHealthCode.LowHP && !isPlayingBattleNOR)
            {
                AudioPlayer.Stop();
                AudioPlayer.PlayOneShot(BattleNOR);
                isPlayingBattleNOR = true;
                isPlayingBattleLOWHP = false; // Ensure BattleLOWHP is not playing
            }
        }
        else
        {
            // Stop all sounds if not in combat
            if (AudioPlayer.isPlaying)
            {
                AudioPlayer.Stop();
            }
            isPlayingBattleNOR = false;
            isPlayingBattleLOWHP = false;
        }
    }
}
