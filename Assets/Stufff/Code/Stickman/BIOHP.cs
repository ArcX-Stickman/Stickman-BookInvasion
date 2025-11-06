using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BIOHP : MonoBehaviour
{

    public Biology BioHP;

    public Image Fill;

    public Slider slide;





   

    // Update is called once per frame
    void Update()
    {
        slide.value = BioHP.biologyHealth;


       




    }
}
