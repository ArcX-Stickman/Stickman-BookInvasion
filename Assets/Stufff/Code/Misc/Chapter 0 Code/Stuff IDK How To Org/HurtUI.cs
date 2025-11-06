using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtUI : MonoBehaviour
{

    public GameObject Hurt;


    public void stop()
    {
       Hurt.gameObject.SetActive(false);
    }
}
