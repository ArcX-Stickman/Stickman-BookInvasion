using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Activation_Through_Animation : MonoBehaviour
{
    public GameObject GameObject;
    public Transform Overide_Position;

    public void Activate()
    {
        if(Overide_Position != null)
        {
            GameObject.transform.position = Overide_Position.position;
            GameObject.transform.rotation = Overide_Position.rotation;

            Overide_Position.gameObject.SetActive(false);
        }
        GameObject.SetActive(true);
    }

    public void DeActivate()
    {
        GameObject.SetActive(false);

    }
}
