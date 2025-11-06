using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Campfire : MonoBehaviour
{

    public GameObject UI;
    public Movement PlayerManager;
    public Health PlayerHealth;
    public GameObject Fade;
    public GameObject SaveIcon;
    public AudioSource AudioP;
    public AudioClip SFX;
    public EncounterManagerAfterLab EncounterManagerAfterLab;
    public bool IsAfterLab;
    public Transform PlayerPos;
    public float PlayerX;
    public float PlayerY;
    public float CampX;
    public float CampY;
    public float Distance;
    public LoadingSystem LoadingSystem;
    public bool WhileStealth;
    private void Start()
    {
        UI.gameObject.SetActive(false);
        Fade.gameObject.SetActive(false);

    }

    private void Update()
    {
        //PlayerX = PlayerPos.position.x;
        //PlayerY = PlayerPos.position.y;
        //CampX =  transform.position.x;
        //CampY = transform.position.y;
        //float XCalc = PlayerX * CampX;
        //float YCalc = PlayerY * CampY;
        //float XSol = Mathf.Pow(XCalc, 2);
        //float YSol = Mathf.Pow(YCalc, 2);
        //Distance = Mathf.Sqrt(XSol * YSol);

        // That Never Workewd :(

        float distance = Vector2.Distance(PlayerPos.position, transform.position);


        //Debug.Log("Distance" + distance);
        if (distance <= 17)
        {

            UI.gameObject.SetActive(true);

        }
        else
        {
            UI.gameObject.SetActive(false);

        }



    }
    public void Rest()
    {
        AudioP.PlayOneShot(SFX);
        PlayerHealth.PlayerHP = PlayerHealth.MaxHP;
        PlayerManager.SaveProgress();
        SaveIcon.gameObject.SetActive(true);
        PlayerHealth.HealCount = 3;
        PlayerHealth.MegaCharge = 30;
        
    }
    private bool StickInRange = false;


    //private void //OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        Debug.LogWarning("Hes here Man");

    //        StickInRange = true;
            

    //        if (PlayerManager.HasKilledEmbry && !IsAfterLab)
    //        {
                
    //            UI.gameObject.SetActive(true);
    //        }
    //        if(IsAfterLab && PlayerManager.FinshedEncounterAfterLab)
    //        {
    //            UI.gameObject.SetActive(true);
    //        }
            
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (!collision.CompareTag("Player"))
    //    {
    //        Debug.LogWarning("Hes Gone Man");
    //        StickInRange = false;
    //        //StartCoroutine(HideUIAfterDelay());

    //        UI.gameObject.SetActive(false);
    //    }
    //}

    public IEnumerator WaitForASec()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        LoadingSystem.LoadGameScene();
    }
    private IEnumerator HideUIAfterDelay()
    {
        Debug.Log("Ye");
        yield return new WaitForSeconds(0.7f); 
        if (!StickInRange)
        {

            UI.gameObject.SetActive(false);
        }
    }




}
