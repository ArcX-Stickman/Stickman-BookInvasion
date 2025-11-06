using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryOBJ : MonoBehaviour
{
    public Sprite[] Sprites;
    private SpriteRenderer SpriteRenderer;
    public bool AlrPicked;
    void Update()
    {
        if (!AlrPicked)
        {
            AlrPicked = true;
            SpriteRenderer = GetComponent<SpriteRenderer>();
            SpriteRenderer.sprite = Sprites[UnityEngine.Random.Range(0, Sprites.Length)];
        }
       
    }

   
}
