using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulseAnim_Shake : MonoBehaviour
{

    public CinemachineImpulseSource _impulseSource;
    public CinemachineImpulseSource[] impulseSourceList;

    public float vel;

    public void Impulse()
    {
        _impulseSource.GenerateImpulse(vel);
    }

    public void ImpulseList(int i)
    {
        impulseSourceList[i].GenerateImpulse(vel);
        Debug.Log("called Impulse from" + impulseSourceList[i].gameObject.name);
    }

    public void ImpulseAnim(float dt)
    {
        _impulseSource.GenerateImpulse(dt);
    }
}
