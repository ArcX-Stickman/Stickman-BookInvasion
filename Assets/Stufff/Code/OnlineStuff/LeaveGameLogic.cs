using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LeaveGameLogic : NetworkBehaviour
{
    public GameObject Player;



    private void Start()
    {
        GameObject stick = GameObject.FindGameObjectWithTag("Player");
        Player = stick;
    }



    public void Leave()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}

