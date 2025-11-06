using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOrHide_AnimationKey : MonoBehaviour
{
    public GameObject Object;

    public void HideIt()
    {
        Object.gameObject.SetActive(false);
    }

    public void GETTT_OUTTT()
    {
        Destroy(Object);
    }

}
