using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSukugaEmbry : MonoBehaviour
{

    public string SceneName;
    public Sprite Sprite;
    public SpriteRenderer Renderer;
    private bool StartE;
    public float time;

    // Update is called once per frame
    void Update()
    {
        if(Renderer.sprite != Sprite) return;
        if (!StartE)
        {
            StartE = true;
            StartCoroutine(E());

        }
    }

    private IEnumerator E()
    {
        yield return new WaitForSecondsRealtime(time);
    }
}
