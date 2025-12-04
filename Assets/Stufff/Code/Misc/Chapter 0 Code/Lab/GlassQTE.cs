using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlassQTE : MonoBehaviour
{

    public GameObject BrokenGlass;
    public GameObject ParticlesGlass;
    

    void OnEnable()
    {
        BrokenGlass.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("ObjectOfDestruction"))
        {
            Destroy(gameObject);
            BrokenGlass.gameObject.SetActive(true);
            ParticlesGlass.gameObject.SetActive(true);
        }
    }
}
