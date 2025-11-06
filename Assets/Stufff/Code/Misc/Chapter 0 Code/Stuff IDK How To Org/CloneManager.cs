using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneManager : MonoBehaviour
{
    public GameObject OG;

    public bool OG_Still_There;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(OG == null)
        {
            OG_Still_There = false;
            Destroy(gameObject);
        }
        else
        {
            OG_Still_There = true;
        }

       if(!OG_Still_There)
        {
            Destroy(gameObject);
        }

    }
}
