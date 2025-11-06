using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScienceHouse : MonoBehaviour
{

    public Movement PLayerManager;
    public bool PlayerHasKey;
    public AudioSource AudioP;
    public AudioClip Locked;
    public AudioClip Open;
    public float Distance;
    public Transform PlayerPOS;
    public GameObject UI;
    public float MinDis;
    public void Update()
    {

        float DistanceCalculation =  Vector2.Distance(PlayerPOS.position , transform.position);
        Distance = DistanceCalculation;
        if(DistanceCalculation <= MinDis)
        {
            UI.gameObject.SetActive(true);
        }
        else
        {
            UI.gameObject.SetActive(false);
        }


    }
    public void OpenDoor()
    {
        
        if (PlayerHasKey)
        {
            Debug.Log("Opening Logic Not Made :(");
        }
        else
        {
            AudioP.PlayOneShot(Locked);
        }
    }

   

    


}
