using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassAnimationCode : MonoBehaviour
{
    public Animator[] GrassAnims;


    public IEnumerator WindyForABit(float speed, float time)
    {
        float ogSpeed = GrassAnims[0].speed;
        foreach (var anim in GrassAnims)
        {
            anim.speed = speed;
        }

        yield return new WaitForSeconds(time);


        foreach (var anim in GrassAnims)
        {
            anim.speed = ogSpeed;
        }


        yield break;
    }
}
