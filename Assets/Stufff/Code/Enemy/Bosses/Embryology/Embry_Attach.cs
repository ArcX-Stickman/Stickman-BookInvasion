using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Embry_Attach : MonoBehaviour
{
    public bool SpecialEventOver_1st;
    public BookAI EnemyAI;
    public float moveSpeed;
    public AudioClip MegaPunchStick;
    public Animator Embry;
    public GameObject BossTheme;
    public GameObject CineCam1;

    public AudioClip QTEButtonPOP;
    public AudioClip ButtonClick;
    public AudioClip Fail;

    public AudioSource AudioP;


    public Animator Embry_QTE_1_AVOID_ZY;
    public Animation Embry_QTE_1_AVOID_ZY_STARTUP;


    public GameObject[] QTEs;



    public Slider slider;

    public Movement movement;

    // Start is called before the first frame update
    void Start()
    {
        BossTheme.gameObject.SetActive(false);

        EnemyAI = GetComponent<BookAI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!SpecialEventOver_1st)
        {
            EnemyAI.enabled = false;
        }


    }
    private bool Start1stEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("StickMelee"))
        {
            if (!SpecialEventOver_1st &&!Start1stEvent)
            {
                Start1stEvent = true;
                StartCoroutine(Special1stEvent());
                BossTheme.gameObject.SetActive(true);
            }
        }
    }

    public IEnumerator Special1stEvent()
    {


        Embry.CrossFade("Sliding",0,0);
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        float t = 0;
        while(t < 6)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            t += Time.deltaTime;

            if(t >= 4.5)
            {
                SpecialEventOver_1st = true;
                EnemyAI.enabled = true;

                Embry.SetTrigger("SlideEnd");
            }
            yield return null;
        }

        /// make the player QTE doging all the zygotes



        EnemyAI.enabled = true;

        yield break;
    }


    public void SwitchCam_1()
    {
        CineCam1.gameObject.SetActive(true);
        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
        brain.m_DefaultBlend.m_Time = 0f;
        TimeManager tm = FindAnyObjectByType<TimeManager>();
        tm.enabled = false;
        Time.timeScale = 0;
       Time.fixedDeltaTime = 0;
        StartCoroutine(QTE_1_AVOID_ZY());
        

        
    }

    public void SwitchOG_Cam()
    {
        movement.enabled = true;
        EnemyAI.enabled = true;

        if (CineCam1 != null)
        {
            CineCam1.gameObject.SetActive(false);

        }
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

    }

    public IEnumerator QTE_1_AVOID_ZY()
    {
        slider.gameObject.SetActive(true);
        slider.maxValue = 100;

        slider.value = slider.maxValue;

        KeyCode[] qteSequence = { KeyCode.Q, KeyCode.Q, KeyCode.D, KeyCode.A };
        List<KeyCode> playerInput = new List<KeyCode>();

        for (int i = 0; i < QTEs.Length && i < qteSequence.Length; i++)
        {
            QTEs[i].gameObject.SetActive(true);

            TextMeshProUGUI text = QTEs[i].GetComponentInChildren<TextMeshProUGUI>();
            text.text = qteSequence[i].ToString();
        }

        float timer = 0f;


        QTE_Attributes QA = Embry_QTE_1_AVOID_ZY.gameObject.GetComponent<QTE_Attributes>();

        int index = 0;
        float qteDuration;
        float qteTimer;
        while (!QA.Ready)
        {


           slider.value -= (1 * Time.unscaledDeltaTime) * 100;

            if (index < qteSequence.Length)
            {
                if (Input.GetKeyDown(qteSequence[index]))
                {
                    playerInput.Add(qteSequence[index]);
                    QTEs[index].SetActive(false);
                    AudioP.PlayOneShot(ButtonClick);

                    index++;
                }
                //  wrong input = fail immediately
                else if (Input.anyKeyDown)
                {
                    KeyCode pressed =
                        Input.GetKeyDown(KeyCode.Q) ? KeyCode.Q :
                        Input.GetKeyDown(KeyCode.A) ? KeyCode.A :
                        Input.GetKeyDown(KeyCode.D) ? KeyCode.D : KeyCode.None;

                    if (pressed != KeyCode.None && pressed != qteSequence[index])
                    {
                        AudioP.PlayOneShot(Fail);
                        // instant fail
                        break;
                    }
                }
            }

            timer += Time.unscaledDeltaTime;
            yield return null;
        }


        // finished QTE
        slider.gameObject.SetActive(false);
        bool success = index == qteSequence.Length;
        Embry_QTE_1_AVOID_ZY.SetBool("Pass", success);
        foreach (GameObject G in QTEs)
        {
            G.SetActive(false);
        }


        // misc
        for (int i = 0; i < playerInput.Count; i++)
        {
            Debug.Log("Player Input List: " + playerInput[i]);
        }

        QTE_Attributes QTE_A = Embry_QTE_1_AVOID_ZY.gameObject.GetComponent<QTE_Attributes>();
        while (!QTE_A.HasFinished)
        {
            yield return null;
        }
        yield return new WaitForSecondsRealtime(0.6f);
        SwitchOG_Cam();
        yield break;
    }
    private bool QTE_Right(KeyCode[] OG, List<KeyCode> Player)
    {
        if (Player.Count < OG.Length)
            return false;

        for (int c = 0; c < OG.Length; c++)
        {
            if (Player[c] != OG[c])
                return false;
        }

        return true;
    }




}
