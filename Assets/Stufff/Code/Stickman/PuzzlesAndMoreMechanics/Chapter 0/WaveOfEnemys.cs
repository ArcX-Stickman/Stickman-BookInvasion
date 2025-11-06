using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveOfEnemys : MonoBehaviour
{
    public GameObject One;
    public GameObject Two;
    public GameObject Three;
    public GameObject Four;
    public GameObject Five;
    public GameObject Collectible;


    // Start is called before the first frame update
    void Start()
    {
        if(Collectible != null)
        {
            Collectible.gameObject.SetActive(false);
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        if(One == null && Two == null &&  Three == null && Four == null && Five == null)
        {
            if(Collectible != null)
            {
                Collectible.gameObject.SetActive(true);
            }
        }
    }
}
