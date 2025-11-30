using System.Collections.Generic;
using UnityEngine;

public class CullingManager : MonoBehaviour
{
    [Tooltip("How often (seconds) to run culling checks for all registered objects.")]
    public float globalCheckInterval = 0.5f; 

    public float managerRange = 0f; 

    private float nextCheckTime;

    class Entry
    {
        public SpriteCulling sc;
        public Transform t;
        public SpriteRenderer sr;
        public BoxCollider2D box;
        public Sprite lowSprite;
        public Sprite highSprite;
        public float maxDistSqr;
        public bool isCulled;
        public float maxDistance;
    }

    private List<Entry> entries = new List<Entry>();

    void Start()
    {
        RefreshAllEntries();
        nextCheckTime = Time.time + Random.Range(0f, globalCheckInterval);
    }

    public void RefreshAllEntries()
    {
        entries.Clear();
        var scs = FindObjectsOfType<SpriteCulling>();
        foreach (var sc in scs)
        {
            if (sc == null || sc.useManager == false) continue;

            var sr = sc.spriteRenderer;
            if (sr == null) sr = sc.GetComponent<SpriteRenderer>();

            var e = new Entry
            {
                sc = sc,
                t = sc.transform,
                sr = sr,
                box = sc.GetComponent<BoxCollider2D>(),
                lowSprite = (sr != null) ? sr.sprite : null,
                highSprite = sc.HighQualitySprite,
                maxDistance = sc.maxDistance,
                isCulled = false
            };
            e.maxDistSqr = e.maxDistance * e.maxDistance;
            entries.Add(e);
        }
    }

    void Update()
    {
        if (Time.time < nextCheckTime) return;
        nextCheckTime = Time.time + globalCheckInterval;

        Transform player = null;
        Movement mv = FindObjectOfType<Movement>();
        if (mv != null) player = mv.transform;
        if (player == null) return;

        Vector2 playerPos = player.position;

        for (int i = 0; i < entries.Count; i++)
        {
            var e = entries[i];
            if (e.sc == null) continue; 

            if (managerRange > 0f)
            {
                float managerDistSqr = (e.t.position - this.transform.position).sqrMagnitude;
                if (managerDistSqr > managerRange * managerRange) continue;
            }

            float distSqr = ((Vector2)e.t.position - playerPos).sqrMagnitude;

            if (distSqr > e.maxDistSqr)
            {
                if (!e.isCulled)
                {
                    if (e.sr != null && e.lowSprite != null)
                        e.sr.sprite = e.lowSprite;

                    if (e.sr != null)
                        e.sr.enabled = false;

                    if (e.box != null)
                        e.box.enabled = false;

                    e.isCulled = true;
                }
            }
            else 
            {
                if (e.isCulled)
                {
                    if (e.sr != null)
                    {
                        if (e.highSprite != null)
                            e.sr.sprite = e.highSprite;

                        e.sr.enabled = true;
                    }

                    if (e.box != null)
                        e.box.enabled = true;

                    e.isCulled = false;
                }
            }
        }
    }

    public void Register(SpriteCulling sc)
    {
        if (sc == null || sc.useManager == false) return;
        foreach (var e in entries) if (e.sc == sc) return;

        var sr = sc.spriteRenderer;
        if (sr == null) sr = sc.GetComponent<SpriteRenderer>();

        var newE = new Entry
        {
            sc = sc,
            t = sc.transform,
            sr = sr,
            box = sc.GetComponent<BoxCollider2D>(),
            lowSprite = (sr != null) ? sr.sprite : null,
            highSprite = sc.HighQualitySprite,
            maxDistance = sc.maxDistance,
            isCulled = false
        };
        newE.maxDistSqr = newE.maxDistance * newE.maxDistance;
        entries.Add(newE);
    }

    public void Unregister(SpriteCulling sc)
    {
        entries.RemoveAll(e => e.sc == sc);
    }
}
