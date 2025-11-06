using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmbrySave : MonoBehaviour
{
    public Movement SaveKnowledge;


    // Start is called before the first frame update
    void Start()
    {
        if (SaveKnowledge.HasKilledEmbry)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SaveKnowledge.HasKilledEmbry)
        {
            Destroy(gameObject);
        }
    }
}
