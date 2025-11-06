using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WantToCheckOwnerThing : NetworkBehaviour
{


   
    void Start()
    {
        if (!IsOwner)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
