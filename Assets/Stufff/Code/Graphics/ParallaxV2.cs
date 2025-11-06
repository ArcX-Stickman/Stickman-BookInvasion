using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxV2 : MonoBehaviour
{


    public Camera cam;
    public Transform Thing;
    Vector2 startpos;
    float Z_start;

    Vector2 travel => (Vector2)cam.transform.position - startpos;

    float DistanceFromThing => transform.position.z - Thing.position.z;
    float clipPane => (cam.transform.position.z + (DistanceFromThing > 0 ? cam.farClipPlane : cam.nearClipPlane));
    float parralaxFactor => Mathf.Abs(DistanceFromThing/clipPane);
    Vector2 ParralaxFactor;

    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position;
        Z_start = transform.position.z;

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newpos = transform.position = startpos + travel;
        transform.position = new Vector3(newpos.x,newpos.y,Z_start);

    }
}
