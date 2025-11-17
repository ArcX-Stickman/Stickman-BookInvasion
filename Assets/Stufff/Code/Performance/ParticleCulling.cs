using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCulling : MonoBehaviour
{
   [Header("Culling Settings")]
    public float maxDistance = 50f;

    [Header("Runtime Debug")]
    public float currentDistance;
    public Transform player;

    private float nextCheckTime;
    private const float checkInterval = 1f;

    public GameObject OBJ;



    void OnEnable()
    {
        if (player == null)
        {
            Movement stick = FindObjectOfType<Movement>();
            if (stick != null)
                player = stick.transform;
        }
        
        nextCheckTime = Time.time + Random.Range(0f, checkInterval);
    }

     

    void Update()
    {
        if (player == null)
        {
            Movement stick = FindObjectOfType<Movement>();
            if (stick != null)
                player = stick.transform;
        }
        if (Time.time >= nextCheckTime && player != null)
        {
            currentDistance = Vector2.Distance(transform.position, player.position);
            nextCheckTime = Time.time + checkInterval;
            bool DisCheckBool = currentDistance <= maxDistance;
            OBJ.gameObject.SetActive(DisCheckBool);
        }
    }
}
