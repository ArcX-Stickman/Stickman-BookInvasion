using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoad : MonoBehaviour
{

    public Movement StickManager;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Saved()
    {
        StickManager.SaveProgress();
        
    }

    public void Load()
    {
        StickManager.LoadProgress();

    }

}
