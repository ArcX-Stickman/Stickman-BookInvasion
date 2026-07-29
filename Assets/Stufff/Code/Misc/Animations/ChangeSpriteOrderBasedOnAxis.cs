using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpriteOrderBasedOnAxis : MonoBehaviour
{
    [Header("Axis")]
    public bool X;
    public bool Y;
    public bool Z;

    [Header("Info")]
    public float Axis;
    public SpriteRenderer Renderer;
    public int Order;

    public bool LessThan;
    public bool GreaterThan;

    private bool Played;

    private void Start()
    {
        if (Renderer == null)
        {
            Renderer = GetComponent<SpriteRenderer>();
        }
    }

    private void Update()
    {
        if (!Played)
        {
            if (transform.position.x == Axis && X || transform.position.x <= Axis && X && LessThan || transform.position.x >= Axis && X && GreaterThan)
            {
                Renderer.sortingOrder = Order;
                Played = true;
            }
            if (transform.position.y == Axis && Y || transform.position.y <= Axis && Y && LessThan || transform.position.y >= Axis && Y && GreaterThan)
            {
                Renderer.sortingOrder = Order;
                Played = true;

            }
            if (transform.position.z == Axis && Z || transform.position.z <= Axis && Z && LessThan || transform.position.z >= Axis && Z && GreaterThan)
            {
                Renderer.sortingOrder = Order;
                Played = true;

            }
        }
        else
        {
            this.enabled = false;
        }

    }
}
