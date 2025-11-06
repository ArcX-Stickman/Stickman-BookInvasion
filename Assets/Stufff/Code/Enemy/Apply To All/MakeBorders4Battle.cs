using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeBorders4Battle : MonoBehaviour
{

    public GameObject BorderL;
    public GameObject BorderR;
    public EnemyAI EnemyAI;

    // Update is called once per frame
    void Update()
    {
        if (EnemyAI == null )
        {
            BorderL.gameObject.SetActive(false);
            BorderR.gameObject.SetActive(false);
        }
        if (!EnemyAI.gameObject.activeSelf)
        {
            BorderL.gameObject.SetActive(false);
            BorderR.gameObject.SetActive(false);
        }
        if (EnemyAI.RangeManager.PlayerSpotted)
        {
            BorderL.gameObject.SetActive(true);
            BorderR.gameObject.SetActive(true);

        }
       
    }
}
