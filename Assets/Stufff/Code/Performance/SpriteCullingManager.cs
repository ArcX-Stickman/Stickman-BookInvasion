using UnityEngine;

public class SpriteCullingManager : MonoBehaviour
{
    [Header("Draw Distances")]
    public float drawDistanceLow = 15f;
    public float drawDistanceMedium = 30f;
    public float drawDistanceHigh = 60f;

    [Header("Mode Toggles (Select ONE)")]
    public bool Low;
    public bool Medium;
    public bool High;

    [Header("Debug")]
    public bool initialized;
    public string currentMode;

    private SpriteCulling[] spriteCullings;

    //private void Update()
    //{
    //    if (!initialized)
    //    {
    //        Initialize();
    //    }

    //    // Detect mode switches safely
    //    if (Low && currentMode != "Low")
    //    {
    //        ApplyDrawDistance(drawDistanceLow);
    //        currentMode = "Low";
    //    }
    //    else if (Medium && currentMode != "Medium")
    //    {
    //        ApplyDrawDistance(drawDistanceMedium);
    //        currentMode = "Medium";
    //    }
    //    else if (High && currentMode != "High")
    //    {
    //        ApplyDrawDistance(drawDistanceHigh);
    //        currentMode = "High";
    //    }
    //}

    //private void Initialize()
    //{
    //    spriteCullings = FindObjectsOfType<SpriteCulling>(true);
    //    initialized = true;
    //}

    //private void ApplyDrawDistance(float value)
    //{
    //    if (spriteCullings == null) return;

    //    foreach (var s in spriteCullings)
    //    {
    //        s.maxDistance = value;
    //    }
    //}
}
