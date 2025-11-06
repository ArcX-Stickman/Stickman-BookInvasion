using UnityEngine;
using System.Collections.Generic;

public class PerformanceAnimatorMonitor : MonoBehaviour
{
    [Tooltip("How often (in seconds) to log the active animators. Set to 0 for manual logging only.")]
    public float logInterval = 1.0f; // Log every 1 second
    private float nextLogTime;

    [Tooltip("Press this key to manually log currently active animators.")]
    public KeyCode manualLogKey = KeyCode.L; // 'L' for Log

    // To store animators that are updating when they might not be expected to be visible
    private HashSet<Animator> alwaysUpdatingAnimators = new HashSet<Animator>();

    void Start()
    {
        nextLogTime = Time.time + logInterval;
    }

    void Update()
    {
        
        Health stick = FindAnyObjectByType<Health>();
        if (stick.Testing)
        {
            if (logInterval > 0 && Time.time >= nextLogTime)
            {
                LogCurrentlyPlayingAnimators();
                nextLogTime = Time.time + logInterval;
            }

            if (Input.GetKeyDown(manualLogKey))
            {
                LogCurrentlyPlayingAnimators();
            }
        }
       
    }

    void LogCurrentlyPlayingAnimators()
    {
        alwaysUpdatingAnimators.Clear(); // Clear for fresh check

        Animator[] allAnimatorsInScene = FindObjectsOfType<Animator>();

        Debug.Log($"--- Checking Animator Culling (Total Animators: {allAnimatorsInScene.Length}) ---");
        int nonCulledCount = 0;

        foreach (Animator animator in allAnimatorsInScene)
        {
            // Only consider animators that are active and enabled in the hierarchy
            if (animator.isActiveAndEnabled)
            {
                bool isActivelyPlaying = false;
                for (int i = 0; i < animator.layerCount; i++)
                {
                    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(i);
                    if (stateInfo.normalizedTime < 1.0f || animator.IsInTransition(i))
                    {
                        isActivelyPlaying = true;
                        break;
                    }
                }

                // Check if it's playing and if it's currently visible
                // Note: 'isVisible' on renderer only tells you if it's in the camera frustum.
                // Animator's update can still happen even if renderer is not visible, depending on cullingMode.
                bool isRendererVisible = true; // Assume visible if no renderer or cullingMode is AlwaysAnimate
                Renderer animatorRenderer = animator.GetComponentInChildren<Renderer>();
                if (animatorRenderer != null)
                {
                    isRendererVisible = animatorRenderer.isVisible;
                }

                // Log animators that are playing, and importantly, their culling mode
                // Also specifically highlight if they are playing but their renderer is NOT visible
                if (isActivelyPlaying)
                {
                    nonCulledCount++;
                    string cullingModeStatus = $"Culling Mode: {animator.cullingMode}";
                    string visibilityStatus = isRendererVisible ? "<color=green>VISIBLE</color>" : "<color=red>NOT VISIBLE (potential culling issue)</color>";

                    string currentClipNames = "";
                    AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
                    if (clipInfo.Length > 0)
                    {
                        foreach (var info in clipInfo)
                        {
                            if (info.clip != null)
                            {
                                currentClipNames += info.clip.name + " ";
                            }
                        }
                        currentClipNames = currentClipNames.Trim();
                    }
                    else
                    {
                        currentClipNames = "No clips in base layer";
                    }

                    Debug.Log($"<color=yellow>GameObject: {animator.gameObject.name}</color> " +
                              $"(Path: {GetGameObjectPath(animator.gameObject)}) | " +
                              $"Playing Clip(s): '{currentClipNames}' | " +
                              $"{cullingModeStatus} | " +
                              $"{visibilityStatus}");

                    if (!isRendererVisible && animator.cullingMode != AnimatorCullingMode.CullCompletely)
                    {
                        // This animator is playing and its renderer is off-screen,
                        // AND its culling mode is NOT CullCompletely, meaning it might still be updating.
                        // This is a prime candidate for optimization.
                        alwaysUpdatingAnimators.Add(animator);
                    }
                }
            }
        }

        if (nonCulledCount > 0)
        {
            Debug.Log($"<color=green>--- Found {nonCulledCount} Actively Playing Animators ---</color>");
        }
        else
        {
            Debug.Log("<color=grey>No Animators currently playing an animation or in transition.</color>");
        }

        if (alwaysUpdatingAnimators.Count > 0)
        {
            Debug.Log($"<color=orange>--- {alwaysUpdatingAnimators.Count} Animators Playing While Renderer NOT Visible ---</color>");
            foreach (var anim in alwaysUpdatingAnimators)
            {
                Debug.Log($"<color=red>Investigate:</color> {anim.gameObject.name} (Culling Mode: {anim.cullingMode})");
            }
        }
        else
        {
            Debug.Log("<color=green>All playing animators with off-screen renderers are being culled effectively (or are using CullCompletely).</color>");
        }
        Debug.Log("------------------------------------");
    }

    private string GetGameObjectPath(GameObject obj)
    {
        string path = "/" + obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path = "/" + obj.name + path;
        }
        return path;
    }
}