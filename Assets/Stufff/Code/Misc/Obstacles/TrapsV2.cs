using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapsV2 : MonoBehaviour
{
    [Header("Player References")]
    public Health StickHP;
    public Movement PlayerManager;

    [Header("Audio")]
    public AudioSource AudioP;
    public AudioClip AudioClip;

    [Header("Trap Settings")]
    public bool TeleportTrap = true;   
    public float DMG = 10f;            

    [Header("Respawn UI")]
    public RespawnUI_ForTraps respawnUI_ForTraps;
    public Animator RespawnUI_BlackFade;



    public bool triggered = false;
    public bool PlsDontGoAwayPapi;
    private void OnEnable()
    {
        triggered = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;
        if (!collision.CompareTag("Player")) return;

        //triggered = true;

        if (AudioP != null && AudioClip != null)
            AudioP.PlayOneShot(AudioClip);

        if (StickHP != null)
            StickHP.PlayerHP -= DMG;

        if (TeleportTrap && PlayerManager != null)
        {
            StartCoroutine(TeleportPlayerToSafety());
        }

        if (!PlsDontGoAwayPapi)
        {
            this.enabled = false;

        }
    }

    private IEnumerator TeleportPlayerToSafety()
    {
        RespawnUI_BlackFade.CrossFade("Respawn_FadeIn_Out_TrapBased", 0, 0);

        yield return new WaitForSecondsRealtime(0.5f);
        if (PlayerManager.SafePosition != Vector3.zero)
        {

            PlayerManager.rb.velocity = Vector2.zero;

            PlayerManager.StopAllCoroutines();

            if (respawnUI_ForTraps == null)
                respawnUI_ForTraps = FindAnyObjectByType<RespawnUI_ForTraps>();

            if (respawnUI_ForTraps != null)
                respawnUI_ForTraps.SafePosition = PlayerManager.SafePosition;

            respawnUI_ForTraps.gameObject.SetActive(true);
            PlayerManager.transform.position = PlayerManager.SafePosition;
        }
        yield break;
    }

    public GameObject GroundLimbs;
    public void SummonTendril_GroundAttack()
    {
        GroundLimbs.gameObject.SetActive (true);
    }
}
