using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAnim : MonoBehaviour
{
    public GameObject UI;


    public void TransitionOUT()
    {
        UI.gameObject.SetActive(false);
    }
}
