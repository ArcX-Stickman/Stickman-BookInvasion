using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScale_MOD : MonoBehaviour
{

    bool waiting = false;
    public void Stop(float duration, float timeScale)
    {
        if (waiting)
        {
            Debug.Log(" stopping failed we returnin to the normal time gang");
            return;
        }
        
        Debug.Log(" stopping success SHRORORORORORO");

        Time.timeScale = timeScale;
        StartCoroutine(Wait(duration));
    }
    public void Stop(float duration)
    {
        Debug.Log("Yo we stopped");
        Stop(duration, 0.0f);
    }

    public void StopAnimKey(float duration)
    {
        Debug.Log("Yo we stopped");
        Stop(duration, 0.0f);
    }


    IEnumerator Wait(float duration)
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
        waiting = false;
    }


    public void SloMO(float duration,float scale)
    {
        Time.timeScale = scale;
        StartCoroutine (Wait(duration));
    }
}
