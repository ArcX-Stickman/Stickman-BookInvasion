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

    private bool isCulled;

    public bool useManager = true;

    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        LowResSprite = spriteRenderer.sprite;

        if (player == null)
        {
            Movement stick = FindObjectOfType<Movement>();
            if (stick != null)
                player = stick.transform;
        }
    }

    void OnEnable()
    {
       
        nextCheckTime = Time.time + Random.Range(0f, checkInterval);
    }

    private void OnDisable()
    {
        if (LowResSprite != null && HighQualitySprite != null)
            spriteRenderer.sprite = LowResSprite;
    }

    void Update()
    {

        if (useManager) return;

        if (player == null)
            return;

        if (Time.time < nextCheckTime)
            return;

        nextCheckTime = Time.time + checkInterval;

        float distSqr = (transform.position - player.position).sqrMagnitude;
        float maxDistSqr = maxDistance * maxDistance;

        currentDistance = Mathf.Sqrt(distSqr);
        if (distSqr > maxDistSqr)
        {
            if (!isCulled)
            {
                spriteRenderer.sprite = LowResSprite;
                spriteRenderer.enabled = false;
                isCulled = true;
            }
        }
        else
        {
            if (isCulled)
            {
                if (HighQualitySprite != null)
                    spriteRenderer.sprite = HighQualitySprite;

                spriteRenderer.enabled = true;
                isCulled = false;
            }
        }
    }
}
