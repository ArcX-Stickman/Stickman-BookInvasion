using UnityEngine;

public class PreventFlipScale : MonoBehaviour
{
    void Update()
    {
        Vector3 currentScale = transform.localScale;

        // Prevent flipping by forcing scale to be positive
        currentScale.x = Mathf.Max(0.1f, Mathf.Abs(currentScale.x));
        currentScale.y = Mathf.Max(0.1f, Mathf.Abs(currentScale.y));
        currentScale.z = Mathf.Max(0.1f, Mathf.Abs(currentScale.z));

        transform.localScale = currentScale;
    }
}
