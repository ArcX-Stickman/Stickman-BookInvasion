using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoSomethingOnCollision : MonoBehaviour
{
    public bool OnlyOnce;
    public bool ChangeCamSize;
    public CameraSukugaCuts cams_Modif;
    public float CamSizeNEW;
    private bool AlrDid;
    public float Cspeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (AlrDid) return;
        if (collision.CompareTag("Player"))
        {
            if (ChangeCamSize)
            {
                cams_Modif.StartCoroutine(cams_Modif.Zoom(CamSizeNEW, Cspeed));
            }

            if (OnlyOnce)
            {
                AlrDid = true;
            }
        }
    }
}
