using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableScientist : MonoBehaviour
{
    public Scientist Scientist;



    // Start is called before the first frame update
    void Start()
    {
        Scientist = GetComponent<Scientist>();
    }

    // Update is called once per frame
    void Update()
    {
        Scientist.enabled = true;
    }
}
