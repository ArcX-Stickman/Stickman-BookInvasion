using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityPresets : MonoBehaviour
{
    // Start is called before the first frame update

    public enum QualityLevels
    {
        Low,
        Medium, 
        High,
        Max
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public class DrawDistance
{
    public float LowDrawDistance = 90;
    public float MediumDrawDistance = 250;
    public float HighDrawDistance = 600;
    public float MaxDrawDistance = 670;

}

public class LightDrawDistance
{
    public float LowLightDis = 30;
    public float MediumLightDis = 100;
    public float HighLightDis = 350;
    public float MaxLightDis = 370;

}
