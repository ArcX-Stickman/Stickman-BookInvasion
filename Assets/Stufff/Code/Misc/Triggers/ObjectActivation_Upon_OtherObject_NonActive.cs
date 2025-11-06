using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActivation_Upon_OtherObject_NonActive : MonoBehaviour
{
    public GameObject ChildObject;
    public GameObject NulledObject;
    void Update()
    {
        if (ChildObject == null) Destroy(gameObject);
        if(NulledObject == null || !NulledObject.activeSelf)
        {
            ChildObject.gameObject.SetActive(true);
        }
    }
}
