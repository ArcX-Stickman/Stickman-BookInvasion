using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePunchVol : MonoBehaviour
{
    public Movement Stick;
    public Embry_Attach Book;

    private void Update()
    {
        if(done) this.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // so da player in vol

            if (Input.GetButtonDown("Fire1"))
            {
                StartCoroutine(ForcePunch());

            }
        }
    }

    private bool done;
    public IEnumerator ForcePunch()
    {
        while (!Book.SpecialEventOver_1st)
        {
            StartCoroutine(Stick.PunchFreeFlow());
            yield return null;

        }
        done = true;
        yield break;
    }
}
