using System.Collections;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Helicopter : MonoBehaviour
{
    public Transform Place_To_Go;
    public Transform Destination;
    public Transform FinalDestination;
    public Transform StickPOS;
    public RangeSight Checker;
    public Movement PlayerManager;
    public Movement StickManager;
    public Health PlayerHP;
    public PauseManager PauseManager;
    public CameraScript CameraScript;
    public GameObject HUD;
    public TextMeshProUGUI HUDText;
    public Animator HUD_ANIM;
    public Animator CameraThing;
    public Animator HeliAnim;
    public Animator ExamAnim;
    public AudioSource PerfectSolu;
    public AudioSource AudioP;
    public AudioClip HelicopterAboveTheme;
    public AudioClip BoomSFX;
    public GameObject Fire;
    public GameObject Wind;
    public GameObject Boom;
    public float Speed = 100f;
    public float MinDIS = 20f;
    public float ExamHealth = 100f;
    private float DestinationMinDIs = 89f;
    private bool DidItAlready;
    private bool HUDTIP;
    private bool AlrMusic;
    public float DistanceFromStickBoi;
    private float DistanceFromDestination;
    public bool ForcedSpeed;
    public bool ExplosionTime;
    public Transform PlayerTeleporter;
    public GameObject WhiteFade;
    public bool Failed;
    public AudioSource BG_Music;
    public AudioSource LowHPCopterThemeVari;
    private void Start()
    {
        ReGen = false;
    }

    private void Update()
    {
        if(Time.timeScale > 0)
        {
            if (ExamAnim != null && !ForcedSpeed)
            {
                if (StickManager == null && PlayerHP != null)
                    StickManager = PlayerHP.gameObject.GetComponent<Movement>();

                DistanceFromDestination = Vector2.Distance(transform.position, FinalDestination.position);
                if (DistanceFromDestination <= 30)
                {
                    WhiteFade.gameObject.SetActive(true);
                    Failed = true;
                    PlayerManager.StickHP.PlayerHP = 0;
                    PlayerManager.OnLadder = false;
                    StickManager.isFallingHeli = true;
                    HeliAnim.SetTrigger("Boom");
                    BG_Music.enabled = false;
                }

                if (!Fire.activeSelf)
                {
                    if (DistanceFromStickBoi >= 100)
                    {
                        Speed = Mathf.Abs(StickManager.rb.velocity.x + 20);

                    }
                    else if (DistanceFromStickBoi >= MinDIS)
                    {
                        Speed = Mathf.Abs(StickManager.rb.velocity.x - 5);

                    }
                    else
                    {
                        Speed = Mathf.Abs(StickManager.rb.velocity.x - 15);

                    }
                }
                else if (Fire.activeSelf && !StickManager.OnLadder && !StickManager.InHeliCineBeatdown && !StickManager.HeliFinale)
                {
                    Speed = 100;
                    Place_To_Go = Destination;
                }
                else if (StickManager.OnLadder || StickManager.HeliFinale)
                {
                    Speed = 150;

                    Wind.SetActive(StickManager.StickHP.PlayerHP > 0);

                    Place_To_Go = FinalDestination;
                    StickManager.HeliFinale = true;
                }
                else
                {
                    Speed = 100;
                }

                if (ExamHealth <= 0)
                {
                    PlayerHP.PlayerHP = 100;
                }

                if (ReGen)
                    PlayerHP.MegaCharge += 0.3f;

                PlayerHP.MegaCharge = 100;

                if (Checker.StickHere)
                {
                    HandleEncounter();
                }
            }
            else if (ExplosionTime)
            {
                StartCoroutine(Explosion());
            }
            else
            {
                if (!ForcedSpeed)
                {
                    Speed = 150; // default slow speed :)

                }
                HandleEncounter();

            }
        }
       

    }

    public void BOOM_SFX()
    {
        AudioP.PlayOneShot(BoomSFX);

    }
    public IEnumerator Explosion()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, StickPOS.position);
        ForcedSpeed = true;
        Speed = 140;
        MinDIS = 15;
        Vector2 FakeStickPos = new Vector2(StickPOS.position.x,transform.position.y);
        while (distanceToPlayer >= MinDIS )
        {
            distanceToPlayer = Vector2.Distance(transform.position, StickPOS.position);
            transform.position = Vector2.MoveTowards(transform.position, FakeStickPos, Speed * Time.deltaTime);
            yield return null;
        }
        if(distanceToPlayer <= MinDIS)
        {
            HeliAnim.SetTrigger("Boom");

        }

        // SaveSystem.ResetGame();
        //PlayerData data = SaveSystem.LoadSave();
        //data.PlayerPOS[0] = PlayerTeleporter.position.x;
        //data.PlayerPOS[1] = PlayerTeleporter.position.y;
        //data.PlayerPOS[2] = PlayerTeleporter.position.z;
       
    }


    public IEnumerator TeleportPlayerToNewScene()
    {
        if (!Failed)
        {
            WhiteFade.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(2);
            PlayerManager.OverrideSave_Position(PlayerTeleporter.position.x, PlayerTeleporter.position.y, PlayerTeleporter.position.z);
            string sceneToLoad = "HeliForestChase";
            // SaveSystem.SaveProgess(StickManager);
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneToLoad);
            loadOperation.allowSceneActivation = false;


            while (loadOperation.progress < 0.9f)
                yield return null;

            loadOperation.allowSceneActivation = true;

            yield return null;
        }
        else
        {
            yield break;
        }
       
    }


    private void HandleEncounter()
    {
        if (ExplosionTime)
        {
            StartCoroutine (Explosion());
            return;
        }
        if (!AlrMusic)
        {
            StickManager.CineMaticSpeed = 100;
            StickManager.MaxCineMaticSpeed = 107;
            StickManager.InCinematic = true;
            StickManager.InExamBoss = true;
            PlayerManager.SceneName = SceneManager.GetActiveScene().name;
            StickManager.Checkpoint = StickManager.ExamHeliID_Checkpoint;
            StickManager.SaveProgress();
            ExamAnim.SetBool("Sniper", true);
            PerfectSolu.clip = HelicopterAboveTheme;
            PerfectSolu.Play();
            AlrMusic = true;
        }

        if(PerfectSolu.clip == HelicopterAboveTheme)
        {
            if(PlayerHP.PlayerHP >= 60)
            {
                PerfectSolu.mute = false;
                LowHPCopterThemeVari.mute = true;
            }
            else
            {
                LowHPCopterThemeVari.mute = false;
                PerfectSolu.mute = true;
            }
        }


        float distanceToPlayer = Vector2.Distance(transform.position, StickPOS.position);
        DistanceFromStickBoi = distanceToPlayer;
        PlayerManager.InExamBoss = true;

        if (distanceToPlayer <= MinDIS)
        {
            ReGen = true;

            if (!DidItAlready)
            {
                DidItAlready = true;
                Place_To_Go = Destination;
                ExamAnim.SetBool("Sniper", true);
            }

            PlayerHP.changerate += 0.02f;
            CameraThing.SetBool("Chase", true);
        }

        if (StickManager.StickHP.PlayerHP <= 0) return;

        if (distanceToPlayer <= MinDIS && !Fire.activeSelf)
        {
            MoveTo(new Vector2(Place_To_Go.position.x, transform.position.y));
        }
        else if (!Fire.activeSelf)
        {
            MoveTo(new Vector2(StickPOS.position.x, transform.position.y));
        }
        else if (Fire.activeSelf)
        {
            MoveTo(new Vector2(Place_To_Go.position.x, transform.position.y));
        }
        else if (StickManager.OnLadder || StickManager.HeliFinale)
        {
            if (DistanceFromDestination >= DestinationMinDIs)
            {
                MoveTo(new Vector2(FinalDestination.position.x, transform.position.y));
            }
        }
    }

    private void MoveTo(Vector2 targetPosition)
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPosition,
            Speed * Time.unscaledDeltaTime
        );
    }

    public bool ReGen { get; private set; }
}
