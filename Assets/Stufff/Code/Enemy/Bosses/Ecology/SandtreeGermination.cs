using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandtreeGermination : MonoBehaviour
{

    public float Distance;
    public float MinDis;
    public Transform PlayerPOS;
    public GameObject GerminationSeed;
    public float SeedSpeed;
    public Transform AimPoint;
    private float LastSeedSpawn;
    public float SeedCD;
    public GameObject Ecology;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Distance = Vector2.Distance(transform.position, PlayerPOS.position);
        if(Ecology != null )
        {
            if (Distance <= MinDis && Time.time >= LastSeedSpawn + SeedCD && Ecology.gameObject.activeSelf)
            {
                LastSeedSpawn = Time.time;
                GerminationCalc();
            }
        }
         
    }


    public void GerminationCalc()
    {
        float angle = Mathf.Atan2(transform.position.y - PlayerPOS.transform.position.y, transform.position.x - PlayerPOS.transform.position.x);
        float X_Axis = Mathf.Cos(angle) * SeedSpeed;
        float Y_Axis = Mathf.Sin(angle) * SeedSpeed;    
        GameObject Seed = Instantiate(GerminationSeed,AimPoint.position,Quaternion.identity);
        Transform SeedPOS = Seed.GetComponent<Transform>();
        GerminationSeed_Manager SeedScript = Seed.GetComponent<GerminationSeed_Manager>();
        SeedScript.isOG = false;
        Vector3 POS_Calc = new Vector3(X_Axis, Y_Axis) * Time.deltaTime;
        SeedScript.POS = POS_Calc;


    }
}
