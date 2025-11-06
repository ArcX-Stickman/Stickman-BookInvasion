using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallLoadLevel : MonoBehaviour
{

    public bool LoadHideOut;
    public LoadLevel LoadLevel;
    private bool AlrDID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !AlrDID)
        {
            AlrDID = true;
            if (LoadHideOut)
            {
                Debug.Log("Yo We loaded the hideout");
                LoadLevel.StartCoroutine(LoadLevel.LoadHideOut());
            }

        }
    }
}
