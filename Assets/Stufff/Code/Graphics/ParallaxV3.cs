using UnityEngine;

//[ExecuteAlways]
public class ParallaxV3 : MonoBehaviour
{
    [Tooltip("0 = far away (barely moves), 1 = same as camera (no parallax).")]
    [Range(0f, 1f)] public float parallaxStrength = 0.5f;

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

        // Get how far the camera moved from its starting point
        Vector3 camDelta = cam.position - camStartPos;

        if (!affectVertical)
            camDelta.y = 0;

        // Calculate new position relative to original placement
        Vector3 newPos = startPos + camDelta * parallaxStrength;

        transform.position = newPos;
    }
}
