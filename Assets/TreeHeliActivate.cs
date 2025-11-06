using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeHeliActivate : MonoBehaviour
{
    public Movement StickManager;
    private bool WantTrees;
    public GameObject Prefab;

    void Update()
    {
        WantTrees = !StickManager.InHeliCineBeatdown;
        if (WantTrees)
        {
            Prefab.gameObject.SetActive(true);
        }
        else
        {
            Prefab.gameObject.SetActive(false);

        }
    }
}
