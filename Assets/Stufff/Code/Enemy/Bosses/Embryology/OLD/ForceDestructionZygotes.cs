using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceDestructionZygotes : MonoBehaviour
{
    public DestroyOnCollisionWithTag[] DestroyOnCollisionWithTag;
    public void ForceDestruction()
    {
        foreach (var item in DestroyOnCollisionWithTag)
        {
            item.SFXPlay();
        }
        this.enabled = false;
    }
}
