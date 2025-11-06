using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperDataHelper : MonoBehaviour
{
    public PaperCollectable PaperManager;
    public GameObject BackflipPaper;
    public Movement PlayerManager;
    public GameObject InfoPaper;
    public GameObject NoMoreStick;
    // UI Texts
    public GameObject BackflipText;
    public GameObject InfoText;


    void Start()
    {
        if (PlayerManager.BackflipCollected)
        {
            BackflipText.gameObject.SetActive(true);
            Destroy(BackflipPaper);
        }
        else
        {
            BackflipText.gameObject.SetActive(false);
        }

        if (PlayerManager.InfoCollected)
        {
            InfoText.gameObject.SetActive(true);

            Destroy(InfoPaper);
        }
        if(PlayerManager.NoMoreCollected)
        {
            NoMoreStick.gameObject.SetActive(true);
            Destroy(NoMoreStick);
        }
        else
        {
            InfoText.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.BackflipCollected)
        {
            BackflipText.gameObject.SetActive(true);

            Destroy(BackflipPaper);
        }
        else
        {
            BackflipText.gameObject.SetActive(false);
        }


        if (PlayerManager.InfoCollected)
        {
            InfoText.gameObject.SetActive(true);

            Destroy(InfoPaper);
        }
        else
        {
            InfoText.gameObject.SetActive(false);
        }
    }


    //public void LoadData()
    //{
    //    PaperData data = SaveSystem.LoadSave();
    //}


}
