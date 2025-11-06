using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BrainSpawnHelp : MonoBehaviour
{

    public GameObject BrainSpawnCheckPoint;
    public GameObject Player;
    public void TellPLayerToGO()
    {
        Player.transform.position = BrainSpawnCheckPoint.transform.position;
        Movement playerManager = Player.GetComponent<Movement>();
        playerManager.SaveProgress();
    }


    public void LoadBrainLevel()
    {
        SceneManager.LoadScene("StickBrain");

    }
}
