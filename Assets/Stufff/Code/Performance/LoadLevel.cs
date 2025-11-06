using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{

    public StickBrain StickBrain;
    public Health StickHP;
    private string scene = "";

    public void Update()
    {
        if(StickHP.PlayerHP <= 0)
        {
            StopAllCoroutines();
        }
    }
    public IEnumerator LoadHideOut()
    {
        if(StickHP.PlayerHP > 0)
        {
            scene = "HideOut";
        }
        else
        {
            scene = "";
        }
        Debug.Log("Load HideOut IEnumerator called");
        AsyncOperation loadOperation =  SceneManager.LoadSceneAsync(scene);
        
        loadOperation.allowSceneActivation = false;
        while (StickBrain.Health >= 0)
        {
            if (StickHP.PlayerHP > 0)
            {
                scene = "HideOut";
            }
            else
            {
                scene = "";
            }
            yield return null;
        }

        yield return new WaitForSecondsRealtime(9);
        loadOperation.allowSceneActivation = true;
    }



}

