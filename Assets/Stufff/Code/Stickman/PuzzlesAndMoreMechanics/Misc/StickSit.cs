using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class StickSit : MonoBehaviour
{
    public DynamicBattleMusic DBM;
    public Movement StickBoi;
    public GameObject StickBoiObj;

    public float DistanceFromPlayer;
    public float MinDis;

    private bool CanSit;

    public AudioSource AudioP;
    public AudioClip SitSFX;

    public GameObject SitText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(DBM !=null && DBM.Enemies.Count > 0)
        {

            CanSit = false;
           // so player cant sit ans chill while enemies are fighting and beating his butt

        }
        else
        {
            if(DistanceFromPlayer <= MinDis)
            {
                CanSit=true;
            }
            else
            {
                CanSit = false;
            }


            if(CanSit && !StickBoi.isSitting)
            {
                SitText.gameObject.SetActive(true);
            }
            else
            {
                SitText.gameObject.SetActive(false);

            }
        }


    }


    public void Sit()
    {
        if(CanSit && !StickBoi.isSitting)
        {
            AudioP.PlayOneShot(SitSFX);
            StickBoi.rb.simulated = false;
            StickBoi.isSitting = true;
            StickBoi.enabled = false;
            if (!PlayerSat)
            {
                PlayerSat = true;
                StartCoroutine(MovePlayerToSeat());
            }
        }
    }

    public void Leave()
    {
        StickBoi.rb.simulated = true;
        StickBoi.isSitting =  false;
        StickBoi.enabled = true;
        
    }

    private bool PlayerSat;
    public IEnumerator MovePlayerToSeat()
    {
        PlayerSat = true;
        float Dis = Vector2.Distance(StickBoiObj.transform.position, transform.position);
        while(Dis > 0.1f)
        {
            StickBoiObj.transform.position = Vector2.MoveTowards(StickBoiObj.transform.position,transform.position,Time.timeScale * 6);
            yield return null;
        }
        StartCoroutine(HealPlayer());
        yield break;
    }


    public IEnumerator HealPlayer()
    {
        StickBoi.StickHP.AudioP.PlayOneShot(StickBoi.StickHP.HealSFX);
        while(StickBoi.StickHP.PlayerHP <= 100)
        {
            StickBoi.StickHP.PlayerHP += 20 * Time.deltaTime;
        }
        yield break;
    }

    
}
