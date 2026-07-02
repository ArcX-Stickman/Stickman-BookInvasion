using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterCertainTime : MonoBehaviour
{

    public bool CloneLogic;
    private float timeS;
    public float duration;
    private void Start()
    {
        timeS = Time.time;
    }

    private void OnEnable()
    {
        timeS = Time.time;
    }
    void Update()
    {
        if (CloneLogic)
        {
            if (gameObject.name.Contains("(Clone)") &&  Time.time - timeS >= duration)
            {
                Destroy(gameObject);
            }
        }
    }
}
