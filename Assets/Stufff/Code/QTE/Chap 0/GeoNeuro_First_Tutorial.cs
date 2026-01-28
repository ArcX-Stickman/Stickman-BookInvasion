using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GeoNeuro_First_Tutorial : MonoBehaviour
{
    public Animator GeoAnim;
    public Animator NeuroAnim;
    public Movement StickManager;
    public GameObject ObjThatTriggers;
    public bool Failed;
    private bool QTEStarted;
    public List<Projectiles> ProjectilesList;
    public AudioSource SloMoAudio;
    public AudioSource BattleTheme;
    public StickEnterTree[] stickEnterTrees;

    public TextMeshProUGUI TutTXT;
    public TutorialKeyInput TKI;
    void Update()
    {
        if(StickManager.Camp1ClearedCH0)
        {
            ObjThatTriggers.SetActive(false);
            this.enabled = false;
        }
        if (ObjThatTriggers == null && !QTEStarted)
        {
            QTEStarted = true;
            StartCoroutine(QTE());
            foreach (StickEnterTree SET in stickEnterTrees)
            {
                SET.enabled = false;
                SpriteRenderer SR = SET.gameObject.GetComponent<SpriteRenderer>();
                Color color = SR.color;
                color.a = SET.OpacityThing;
                SR.color = color;
            }
        }
    }

    public IEnumerator QTE()
    {
        bool finished = false;

        StickManager.enabled = false;

        float originalFixedDelta = Time.fixedDeltaTime;

        while (!finished)
        {
            StickManager.rb.velocity = Vector2.zero;
            StickManager.rb.simulated = false;
            StickManager.ResetAllAnimationStates();
            StickManager.enabled = false;

            float PlayerStartHP = StickManager.StickHP.PlayerHP; // if theeres a change in health thats negative it means player got damaged and failed

            GeoAnim.SetBool("IdleInterupt", true);
            NeuroAnim.SetBool("IdleInterupt", true);

            foreach (StickEnterTree SET in stickEnterTrees)
            {
                SET.enabled = false;
                SpriteRenderer SR = SET.gameObject.GetComponent<SpriteRenderer>();
                Color color = SR.color;
                color.a = SET.OpacityThing;
                SR.color = color;
            }

            float projectileWaitTimer = 0f;
            while (ProjectilesList.Count < 3)
            {
                projectileWaitTimer += Time.deltaTime;
                if (projectileWaitTimer >= 7f)
                {
                    Failed = true;
                    finished = true;
                    break;
                }
                yield return null; // wait for all of geology's projectiles to add to the list
            }

            if (finished)
                break;

            bool CanCountinue = false;
            bool DistanceReached = false;

            float AvgDis = 0;

            while (!DistanceReached)
            {
                foreach (StickEnterTree SET in stickEnterTrees)
                {
                    SET.enabled = false;
                    SpriteRenderer SR = SET.gameObject.GetComponent<SpriteRenderer>();
                    Color color = SR.color;
                    color.a = SET.OpacityThing;
                    SR.color = color;
                }

                StickManager.enabled = false;

                AvgDis = 0;
                float sum = 0;
                int validCount = 0;

                foreach (Projectiles projectile in ProjectilesList)
                {
                    if (projectile != null)
                    {
                        sum += Vector2.Distance(projectile.transform.position, StickManager.transform.position); // get the distance of the prohjectiles
                        validCount++;
                    }
                }

                if (validCount > 0)
                    AvgDis = sum / validCount;

                if (AvgDis <= 18)
                {
                    DistanceReached = true;
                }

                yield return null;
            }

            bool PlayerClicked = false;
            float inputTimer = 0f;
            TKI.gameObject.SetActive(true);
            TKI.InputS = KeyCode.Q;
            TutTXT.text = "Backflip [Q]";
            CameraScript CS = FindAnyObjectByType<CameraScript>();
            float TempCam = CS.CamSize;
            CS.ZoomTo(25, 0.5f);
            while (!PlayerClicked)
            {
                foreach (StickEnterTree SET in stickEnterTrees)
                {
                    SET.enabled = false;
                    SpriteRenderer SR = SET.gameObject.GetComponent<SpriteRenderer>();
                    Color color = SR.color;
                    color.a = SET.OpacityThing;
                    SR.color = color;
                }

                StickManager.enabled = false;

                BattleTheme.gameObject.SetActive(false);
                SloMoAudio.gameObject.SetActive(true);
                StickManager.TimeManager.enabled = false;

                Time.timeScale = 0;
                Time.fixedDeltaTime = originalFixedDelta * Time.timeScale;

                inputTimer += Time.unscaledDeltaTime;
                if (inputTimer >= 1.5f)
                {
                    Failed = true;
                    finished = true;
                    break;
                }

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    StickManager.rb.simulated = true;
                    StickManager.enabled = true;
                    StickManager.StartCoroutine(StickManager.BackFlipMove());
                    PlayerClicked = true;
                }

                yield return null;      
            }
            TKI.gameObject.SetActive(false);
            CS.ZoomTo(TempCam, 0.3f);

            Time.timeScale = 1;
            Time.fixedDeltaTime = originalFixedDelta;

            StickManager.TimeManager.enabled = true;

            BattleTheme.gameObject.SetActive(true);
            SloMoAudio.gameObject.SetActive(false);

            if (StickManager.StickHP.PlayerHP < PlayerStartHP)
            {
                Failed = true;
                finished = true;
                break;
            }

            BookAI bookAI = NeuroAnim.gameObject.GetComponent<BookAI>();
            bookAI.EnemVision.PlayerSpotted = true;

            foreach (StickEnterTree SET in stickEnterTrees)
            {
                SET.enabled = true;
            }

            finished = true;
            yield return null;
        }

        StickManager.rb.simulated = true;
        StickManager.enabled = true;
        Time.timeScale = 1;
        Time.fixedDeltaTime = originalFixedDelta;

        yield break;
    }
}
