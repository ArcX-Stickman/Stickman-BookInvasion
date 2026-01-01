using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObject_On_Enable : MonoBehaviour
{
    public GameObject ObjectToActivate;
    public bool WantExactPositionOnStart;
    public bool AlwaysWantFollowPosition;

    private void OnEnable()
    {
        ObjectToActivate.gameObject.SetActive(true);
        if (WantExactPositionOnStart)
        {
            ObjectToActivate.transform.position = transform.position;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!AlwaysWantFollowPosition)
        {
            return;
        }
    }
}
