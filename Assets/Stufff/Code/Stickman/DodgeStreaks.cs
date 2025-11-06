using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeStreaks : MonoBehaviour
{
    public float streak;

    public Health PlayerManager;


    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void StreakThing()
    {
        PlayerManager.MegaCharge = PlayerManager.MegaCharge + 2 + (streak * PlayerManager.changerate);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
