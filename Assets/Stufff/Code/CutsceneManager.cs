using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public GameObject CutsceneStart;
    public Movement Player;
    public bool FirstCutscene;
    public GameObject IntroHint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.Cutscene_IntroFight && !Player.NeedsToViewDirecFirstIntro && Player.HasFullyWatchedFirst)
        {
            IntroHint.gameObject.SetActive(true);
            Player.NeedsToViewDirecFirstIntro = true;
            Player.SaveProgress();

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Player.Cutscene_IntroFight != true)
            {

                CutsceneStart.gameObject.SetActive(true);
                if (FirstCutscene)
                {
                    Player.Cutscene_IntroFight = true;
                    

                }
                Player.SaveProgress();
            }
            

        }
    }

    public void ReloadScene()
    {
        Player.StartCoroutine(Player.ImmediateReloadGameScene());
    }
}
