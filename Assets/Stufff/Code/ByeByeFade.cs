using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ByeByeFade : MonoBehaviour
{
    public void Byeeee()
    {
        Destroy(gameObject);
    }

    public void Cya()
    {
        gameObject.SetActive(false);
    }
}
