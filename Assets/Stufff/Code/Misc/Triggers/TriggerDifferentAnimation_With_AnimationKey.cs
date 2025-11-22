using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDifferentAnimation_With_AnimationKey : MonoBehaviour
{
    public Animator[] animator;
    public string AnimationName;

    public void TriggerAnim()
    {
        
        if(animator != null)
        {
            foreach(var anim in animator)
            {
                anim.cullingMode = AnimatorCullingMode.AlwaysAnimate;
                anim.CrossFade(AnimationName, 0, 0);
            }
        }
        else
        {
            this.enabled = false;
        }
    }

}
