using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleV2 : MonoBehaviour
{
    public Health thing;
    public GameObject BattleNOR;
    public GameObject LowHP;
    public GameObject NotInCombat;
    public GameObject BiologyFight;


    public bool BioFight;
    public bool NormalCombat;
    public bool LowHp;
    public bool NoCombat;
    public bool GeoGlitch;
    public bool HypothesisCanPlay;


    // Start is called before the first frame update
    void Start()
    {
        NotInCombat.gameObject.SetActive(true);
        BattleNOR.gameObject.SetActive(false);
        LowHP.gameObject.SetActive(false);
        BiologyFight.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        //if (thing.BiologyBoss == true)
        //{
        //    BioFight = true;
        //    BiologyFight.gameObject.SetActive(true);


        //}

        //if(GeoGlitch == true)
        //{
        //    LowHp = false;
        //    NormalCombat = false;
        //    BioFight = false;
        //    NoCombat = true;
        //}

        //if(GeoGlitch == true)
        //{
        //    Debug.Log("GEOGLITCH SOLVED");
        //    NoCombat = true;
        //}

        //if (BioFight == true)
        //{
        //    LowHp = false;
        //    NormalCombat = false;
        //    NoCombat = false;
        //}
        
        //if (NormalCombat == true)
        //{
        //    LowHp = false;
        //    BioFight = false;
        //    NoCombat = false;
        //}
        //if (NoCombat == true )
        //{
        //    LowHp = false;
        //    BioFight = false;
        //    NoCombat = false;
        //    NormalCombat = false;
        //    GeoGlitch = false;
        //}

        //if (thing.InCombat == false)
        //{
        //    NoCombat = true;
        //}






        //if (LowHp == true && thing.InCombat)
        //{
        //    LowHP.gameObject.SetActive(true);
        //}
        //else
        //{
        //    LowHP.gameObject.SetActive(false);
        //}

        //if (NoCombat)
        //{
        //    NotInCombat.gameObject.SetActive(true);
        //}
        //else
        //{
        //    NotInCombat.gameObject.SetActive(false);
        //}
        //if (NormalCombat && thing.InCombat)
        //{
        //    BattleNOR.gameObject.SetActive(true);
        //}
        //else
        //{
        //    BattleNOR.gameObject.SetActive(false);
        //}
        //if (BioFight)
        //{
        //    BiologyFight.gameObject.SetActive(true);
        //}
        //else
        //{
        //    BiologyFight.gameObject.SetActive(false);
        //}



    }


}











//if (thing.InCombat == true)
//{
//    if (thing.BiologyBoss == true)
//    {
//        NotInCombat.gameObject.SetActive(false);
//        BattleNOR.gameObject.SetActive(false);
//        LowHP.gameObject.SetActive(false);
//        BiologyFight.gameObject.SetActive(true);
//    }
//    else
//    {
//        BattleNOR.gameObject.SetActive(true);
//        NotInCombat.gameObject.SetActive(false);
//        Debug.Log("Normal Fight");
//        if (thing.PlayerHP < 30)
//        {
//            BattleNOR.gameObject.SetActive(false);
//            LowHP.gameObject.SetActive(true);
//            NotInCombat.gameObject.SetActive(false);
//        }
//    }




//            }
//        { 




//        }
//        if(thing.InCombat == false)
//        {
//            BattleNOR.gameObject.SetActive(false);
//            LowHP.gameObject.SetActive(false);
//            NotInCombat.gameObject.SetActive(true);

//            if(thing.BiologyBoss == true)
//            {
//                NotInCombat.gameObject.SetActive(false);
//            }



//        }

//    }
//}
