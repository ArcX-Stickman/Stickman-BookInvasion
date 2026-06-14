using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidPointTransform : MonoBehaviour
{
    public Transform P1;
    public Transform P2;
    private Vector2 Midpoint;

    private void FixedUpdate()
    {
        if(!P1.gameObject.activeSelf || !P2.gameObject.activeSelf) return;
        float X = (P1.position.x + P2.position.x)/2; 
        float Y = (P1.position.y + P2.position.y)/2;
        Midpoint = new Vector2(X, Y);
        transform.position = Midpoint;
    }
}
