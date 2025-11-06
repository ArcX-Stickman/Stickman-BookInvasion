using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionBlurForAll : MonoBehaviour
{
    public Rigidbody2D rb;
    public MotionBlur motionBlurHandler;
    public bool blur;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude > 0.1f)
        {
            motionBlurHandler.HandleMotionBlur();
        }
        if (blur)
        {
            motionBlurHandler.HandleMotionBlur();
        }

    }
}
