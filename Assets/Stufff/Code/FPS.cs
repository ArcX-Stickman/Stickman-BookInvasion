using TMPro;
using UnityEngine;

public class FPS : MonoBehaviour
{
    public TextMeshProUGUI fpsDisplay;

    private float pollingTime = 1f;
    private float time;
    private int frameCount;

    void Start()
    {
        // Disable VSync
        QualitySettings.vSyncCount = 0;

        // Optionally, you can set a target frame rate
        Application.targetFrameRate = 0; // Set this to a high value or 0 for no limit
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        frameCount++;

        if (time >= pollingTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            fpsDisplay.text = frameRate.ToString() + " FPS";

            time = 0f;
            frameCount = 0;
        }
    }
}
