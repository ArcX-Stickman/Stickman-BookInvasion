using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellWallSummon : MonoBehaviour
{

    public bool isOg;
    public GameObject CellWall;
     public void TeleGraphed()
    {
        if (!isOg)
        {
            GameObject Wall = Instantiate(CellWall, transform.position, Quaternion.identity);
            CellWall cw = Wall.GetComponent<CellWall>();
            cw.GET_OUT = true;
            Destroy(gameObject);
        }
      

    }
}
