using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePunchVol : MonoBehaviour
{
    public Movement Stick;
    public Embry_Attach Book;
    

    private void Update()
    {
        if (Input.GetButtonDown("Fire2") && InVol && !done)
        {
            StartCoroutine(Stick.PunchFreeFlow());
                this.enabled = false;

            }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // so da player in vol
            InVol = true;
           
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
               // so da player not in vol

               InVol = false;
            }
        }

    private bool done;
   
    public IEnumerator ForcePunch()
    {
        while (!Book.SpecialEventOver_1st)
        {
            yield return null;

        }
        done = true;
        yield break;
    }
    private bool InVol;


    //    using System.Collections; // i tried some crap it didnt work so ima keep this here to steal some storgge (JUST JOKING BTW I PROMISE IM NOT)
    //using System.Collections.Generic;  // jk i just like looking at my old crap code
    //using UnityEngine;

    //public class ForcePunchVol : MonoBehaviour
    //{
    //    public Movement Stick;
    //    public Embry_Attach Book;

    //    private void Update()
    //    {

    //        if (Input.GetButtonDown("Fire2") && !done && InVol)
    //        {
    //            StartCoroutine(ForcePunch());

    //        }
    //        if (done) this.enabled = false;
    //    }



    //    private void OnTriggerEnter2D(Collider2D collision)
    //    {
    //        if (collision.CompareTag("Player"))
    //        {
    //            // so da player in vol

    //            InVol = true;
    //        }
    //    }

    //    private void OnTriggerExit2D(Collider2D collision)
    //    {
    //        if (collision.CompareTag("Player"))
    //        {
    //            // so da player not in vol

    //            InVol = false;
    //        }
    //    }

    //    private bool done;
    //    public IEnumerator ForcePunch()
    //    {
    //        while (!Book.SpecialEventOver_1st)
    //        {

    //            Stick.StickAnim.CrossFade("StickAttach", 0, 0);
    //            float DisFromEmBryStick = Vector2.Distance(Stick.transform.position, Book.transform.position);
    //            while (DisFromEmBryStick > 0.1f)
    //            {
    //                DisFromEmBryStick = Vector2.Distance(Stick.transform.position, Book.transform.position);
    //                Stick.enabled = false;
    //                Stick.transform.position = Vector2.MoveTowards(Stick.transform.position, Book.transform.position, 250 * Time.deltaTime);

    //                yield return null;
    //            }
    //            Stick.enabled = true;
    //            Stick.StickAnim.CrossFade("StickAttachPunch", 0, 0);


    //            yield return null;
    //        }




    //        done = true;
    //        yield break;
    //    }


}
