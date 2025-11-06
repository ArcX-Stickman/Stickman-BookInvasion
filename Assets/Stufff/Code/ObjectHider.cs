using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHider : MonoBehaviour
{
    public void ByeBye()
    {
        gameObject.SetActive(false);
    }


    public void DestroyIt()
    {
        Destroy(gameObject);
    }
}
