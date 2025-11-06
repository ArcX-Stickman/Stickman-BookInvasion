using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ecology : MonoBehaviour
{

    public Vision Vision;
    public AudioSource Hypothesis;
    public AudioSource EcologyTheme;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vision.PlayerSpotted)
        {
            EcologyTheme.gameObject.SetActive(true);
            Hypothesis.gameObject.SetActive(false);
        }
        
    }
}
