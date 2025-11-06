using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoEncounterDestroyIt : MonoBehaviour
{
    public Movement SaveKnowledge;
    void Start()
    {
        if (SaveKnowledge.FinshedEncounterAfterLab)
        {
            Destroy(gameObject);
        }
    }

   
}
