using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DevMode : MonoBehaviour
{
    public Movement Stick;
    public GameObject DevPannel;
   public void Forest()
    {
        PlayerData data = SaveSystem.LoadSave();
        Stick.Checkpoint = 0;
        Stick.SaveProgress();
    }
    public void Brain()
    {
        PlayerData data = SaveSystem.LoadSave();
        Stick.Checkpoint = 999;
        Stick.SaveProgress();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && Input.GetKeyDown(KeyCode.F9))
        {
            DevPannel.gameObject.SetActive(true);
        }
    }
    public void HidePannel()
    {
        DevPannel.gameObject.SetActive(false);
    }
}
