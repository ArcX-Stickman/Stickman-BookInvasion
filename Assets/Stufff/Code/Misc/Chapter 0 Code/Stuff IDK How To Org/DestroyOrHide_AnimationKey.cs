using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOrHide_AnimationKey : MonoBehaviour
{
    public GameObject Object;
    public bool isOG;

    private void Start()
    {
        if(Object == null)
        {
            Object = gameObject;
        }
    }
    public void HideIt()
    {
        if (isOG) return;
        Object.gameObject.SetActive(false);
    }

    public void GETTT_OUTTT()
    {
        if (!isOG)
        {
            Destroy(Object);

        }
    }

}
