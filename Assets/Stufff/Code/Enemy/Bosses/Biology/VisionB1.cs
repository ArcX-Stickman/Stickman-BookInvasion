using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionB1 : MonoBehaviour
{
    public bool PlayerSpotted;
    public Health MusicTrigg;

    // Start is called before the first frame update
    void Start()
    {
        PlayerSpotted = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerSpotted = true;
            MusicTrigg.BiologyBoss = true;
            Debug.Log("Vision B1");
        }

    }

    // Update is called once per frame
    void Update()
    {
        //hi
    }
}
