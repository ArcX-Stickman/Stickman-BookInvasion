using Unity.VisualScripting;
using UnityEngine;

public class SpriteCulling : MonoBehaviour
{
    [Header("Culling Settings")]
    public float maxDistance = 50f;

    [Header("Runtime Debug")]
    public float currentDistance;
    public Transform player;
    public SpriteRenderer spriteRenderer;

    private float nextCheckTime;
    private const float checkInterval = 1f;

    public Sprite HighQualitySprite;

    private Sprite LowResSprite;

    void Start()
    {
        LowResSprite = spriteRenderer.sprite;
    }

    void OnEnable()
    {
        if (player == null)
        {
            Movement stick = FindObjectOfType<Movement>();
            if (stick != null)
                player = stick.transform;
        }
        if(HighQualitySprite != null)
        {
            spriteRenderer.sprite = HighQualitySprite;

        }
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        nextCheckTime = Time.time + Random.Range(0f, checkInterval);
    }

     private void OnDisable() {
        if(LowResSprite != null && HighQualitySprite != null)
        {
            spriteRenderer.sprite = LowResSprite;
        }
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
            spriteRenderer.enabled = currentDistance <= maxDistance;
        }
    }
}
