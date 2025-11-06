using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DestroyCutscene : MonoBehaviour
{
    public Movement Stickman;
    public bool Cut1;

    // Update is called once per frame
    void Update()
    {
        if (Cut1 && Stickman.HasFullyWatchedFirst)
        {
            Destroy(gameObject);
        }
    }
}
