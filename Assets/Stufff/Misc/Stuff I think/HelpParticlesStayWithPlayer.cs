using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpParticlesStayWithPlayer : MonoBehaviour
{
    public Transform PlayerPOS;
    private void FixedUpdate()
    {
        transform.position = PlayerPOS.position;
    }
}
