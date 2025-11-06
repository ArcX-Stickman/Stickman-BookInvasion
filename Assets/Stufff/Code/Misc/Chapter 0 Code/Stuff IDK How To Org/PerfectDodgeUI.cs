using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfectDodgeUI : MonoBehaviour
{
    public GameObject UI;
    public Movement PLayerManager;
        
    // Update is called once per frame
    void Update()
    {
        if (PLayerManager.isInPerfectDodge)
        {
            UI.gameObject.gameObject.SetActive(true);
        }
        else
        {
            UI.gameObject.gameObject.SetActive(false);

        }
    }
}
