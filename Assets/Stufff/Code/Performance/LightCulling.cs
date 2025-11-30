using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
[ExecuteAlways]
public class LightCulling : MonoBehaviour
{
    [Header("Culling Settings")]
    public float maxDistance;

    [Header("Runtime Debug")]
    public float currentDistance;
    public Transform player;    
    public Light2D light2D;
    private float nextCheckTime;
    private const float checkInterval = 1f;

    void OnEnable()
    {
        if (player == null)
        {
            Movement stick = FindObjectOfType<Movement>();
            if (stick != null)
                player = stick.transform;
        }

        if (light2D == null)
            light2D = GetComponent<Light2D>();

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
            light2D.enabled = currentDistance <= maxDistance;
        }
    }
}
