using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackLoad : MonoBehaviour
{

    public GameObject Blank;
    public Movement PlayerManager;
    public bool SupposedToLoad;
    public bool AlrDid;


    // Start is called before the first frame update
    void Start()
    {
        if (SupposedToLoad)
        {
            PlayerManager.LoadProgress();

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Reload()
    {
        if (!AlrDid)
        {
            AlrDid = true;
            SceneManager.LoadScene("Game");

        }
    }

    public void DestroyStart()
    {
        Destroy(Blank);
    }

    


}
