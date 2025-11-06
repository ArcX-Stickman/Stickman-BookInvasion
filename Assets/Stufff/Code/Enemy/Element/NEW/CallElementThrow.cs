using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallElementThrow : MonoBehaviour
{
    public Element_Patrol element;
   public void Call()
    {
        element.ThrowAttack();
    }
}
