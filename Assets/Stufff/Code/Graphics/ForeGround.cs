using UnityEngine;
[ExecuteAlways]

public class ForegroundParallax : MonoBehaviour
{
    [Tooltip("0 = camera locked, higher = moves opposite to camera (foreground motion).")]
    [Range(0f, 3f)] public float parallaxStrength = 1.5f;

    [Tooltip("Include Y movement as well.")]
    public bool affectVertical = true;

    private Transform cam;
    private Vector3 startPos;
    private Vector3 camStartPos;

    void Start()
    {
        cam = Camera.main.transform;
        startPos = transform.position;
        camStartPos = cam.position;
    }

    void LateUpdate()
    {
        if (cam == null) return;

        Vector3 camDelta = cam.position - camStartPos;
        if (!affectVertical)
            camDelta.y = 0;

        // Move opposite to camera direction for foreground effect
        Vector3 newPos = startPos - camDelta * parallaxStrength;

        transform.position = newPos;
    }
}
