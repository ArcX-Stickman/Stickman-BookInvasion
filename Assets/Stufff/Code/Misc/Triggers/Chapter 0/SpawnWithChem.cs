using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWithChem : MonoBehaviour
{
    public GameObject Group;
    public Chemistry Chem;

    private void Start()
    {
        Group.gameObject.SetActive(false);

    }
    void Update()
    {
        if (Chem.isShooting)
        {
            Group.gameObject.SetActive(true);
        }
    }
}
