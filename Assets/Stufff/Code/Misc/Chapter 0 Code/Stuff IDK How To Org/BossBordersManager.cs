using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBordersManager : MonoBehaviour
{

    public GameObject BorderPrefab;
    public GameObject BossPrefab;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(BossPrefab == null)
        {
            Destroy(BorderPrefab);
        }
        //if(BossPrefab.gameObject.activeSelf == false)
        //{
        //    BorderPrefab.gameObject.SetActive(false);
        
        //}
        //else if(BossPrefab.gameObject.activeSelf == true)
        //{
        //    BorderPrefab.gameObject.SetActive(true);
        //}


    }
}
