using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableIfPlayerAFK : MonoBehaviour
{
    public Rigidbody2D Player;
    public float Timer;
    public GameObject Enabler;
    public float MaxTimeAFK;
  
    void Update()
    {
        if(Player.velocity == Vector2.zero)
        {
            Timer += Time.deltaTime;
        }
        else
        {
            Timer = 0;
            Enabler.gameObject.SetActive(false);

        }

        if (Timer >= MaxTimeAFK)
        {
            Enabler.gameObject.SetActive(true);
        }
    }
}
