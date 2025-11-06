using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLabChase : MonoBehaviour
{

    public GameObject Explosion;
    public Health PlayerHP;
    public SpriteRenderer GasTank;
    public Transform CheckPoint1;
    public Transform PlayerPOS;
    public GameObject Lab;
    public PauseManager PauseManager;
    public Movement PlayerManager;
    void Start()
    {
        Explosion.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool ALrdid;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GasTank.enabled = false;
            Explosion.gameObject.SetActive(true);
            if (!ALrdid)
            {
                ALrdid = true;
                StartCoroutine(StopLabSceneThing());
            }
        }
    }

    public IEnumerator StopLabSceneThing()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        PauseManager.NoHUD = true;
        PauseManager.Pause();
        PauseManager.BreakIN.UnPause();
        PlayerPOS.position = CheckPoint1.position;
        PlayerManager.SaveProgress();
        
        yield return new WaitForSecondsRealtime(2f);
        PauseManager.Resume();
        PauseManager.NoHUD = false;

        PlayerManager.LoadProgress();
    }





}
