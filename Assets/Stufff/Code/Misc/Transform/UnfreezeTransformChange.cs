using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnfreezeTransformChange : MonoBehaviour
{

    public bool ByCollision;
    public ChangeTransform changeTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ByCollision) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            changeTransform.Frozen = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!ByCollision) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            changeTransform.Frozen = false;
        }
    }
}
