using UnityEngine;

public class Chromatic: MonoBehaviour
{
    public Material material;
    public float offset = 0.005f;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material != null)
        {
            material.SetFloat("_Offset", offset);
            Graphics.Blit(source, destination, material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}

