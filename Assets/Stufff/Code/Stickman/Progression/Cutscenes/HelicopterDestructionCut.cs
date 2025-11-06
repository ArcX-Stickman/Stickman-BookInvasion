using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterDestructionCut : MonoBehaviour
{
    public Animator HeliAnim;

    private void Start()
    {
        HeliAnim.SetBool("Sniper",true);

    }
    public void TriggerAnim_Exam()
    {
        HeliAnim.SetTrigger("Cooked");
    }
}
