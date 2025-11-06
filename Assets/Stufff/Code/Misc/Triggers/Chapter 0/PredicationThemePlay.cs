using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredicationThemePlay : MonoBehaviour
{

    public GameObject Music;
    public BattleV2 MusicManager;
    public GameObject Hypothesis;
    


    // Start is called before the first frame update
    void Start()
    {
        Music.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            Destroy(Hypothesis);
            MusicManager.NoCombat = false;
            Music.gameObject.SetActive(true);
        }
        
    }
}
