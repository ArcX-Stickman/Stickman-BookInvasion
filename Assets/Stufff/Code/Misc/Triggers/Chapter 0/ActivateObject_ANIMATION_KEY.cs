using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObject_ANIMATION_KEY : MonoBehaviour
{

    public GameObject Obj_To_Activate;
    private Health StickHP;

    void Start()
    {
       // StickHP = FindAnyObjectByType<Health>();
        Obj_To_Activate.gameObject.SetActive(false);
    }

    public void Activate()
    {
        StickHP = FindAnyObjectByType<Health>();
        if(StickHP.PlayerHP > 0)
        {
            Obj_To_Activate.gameObject.SetActive(true);

        }

    }

    public void DeActivate()
    {
        Obj_To_Activate.gameObject.SetActive(false);

    }

    public void DestoyITTT()
    {
        Destroy(Obj_To_Activate.gameObject);
    }
}
