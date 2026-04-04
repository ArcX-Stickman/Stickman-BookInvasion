using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Embryology;
    private EnemyAI EmbryAI;
    public GameObject Minion;
    public float CD;
    private float LastCD;
    void Start()
    {
        if (Embryology.gameObject.activeSelf)
        {
            EmbryAI = Embryology.GetComponent<EnemyAI>();

        }
    }
    void Update()
    {
        if(Time.time >= CD + LastCD  && Embryology.gameObject.activeSelf && EmbryAI.PhaseNum == 3)
        {
            LastCD = Time.time;
            GameObject MinionC = Instantiate(Minion,transform.position, Quaternion.identity);
            EmbryMinions minion = MinionC.GetComponent<EmbryMinions>();
            minion.isOg = false;
        }
    }
}
