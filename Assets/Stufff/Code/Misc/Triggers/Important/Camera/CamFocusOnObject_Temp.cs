using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFocusOnObject_Temp : MonoBehaviour
{
    [Header("WARNING: THIS ONLY WORKS WITH OBJECTS THAT ONLY STAY FOR A BIT AND AUTO DESTORY THEMSELVE ")]
    public bool NeedAnim;
    void Update()
    {
        if (!NeedAnim)
        {
            StartCoroutine(Focus());
        }
    }

    public IEnumerator Focus()
    {
    
        CameraScript Cam = FindAnyObjectByType<CameraScript>();
        while(gameObject.activeSelf)
        {
            Cam.StickCam.m_Follow = gameObject.GetComponent<Transform>();
            yield return null;
        }

        yield break;
    }
}
