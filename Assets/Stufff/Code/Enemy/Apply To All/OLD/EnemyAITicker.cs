using UnityEngine;

public class EnemyAITicker : MonoBehaviour
{
    private EnemyAI ai;
    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D col;
    private GameObject SlideUI;

    private Transform player;

    public float MInDIS = 270f;
    public float WakeUpDIS = 180f;


    private bool isActive = true;

    void Awake()
    {
        ai = GetComponent<EnemyAI>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        SlideUI = ai.HPSlide.gameObject;

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;

        }
    }

    void Update()
    {
        if (player == null)
            return;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist > MInDIS && isActive)
        {
            SetActiveState(false);
        }
        else if (dist < WakeUpDIS && !isActive)
        {
            SetActiveState(true);
        }
    }

    private void SetActiveState(bool state)
    {
        isActive = state;

        if (ai != null)
        {
            ai.enabled = state;

        }

        if (anim != null)
        {
            anim.enabled = state;

        }

        if (rb != null)
        {
            rb.simulated = state;

        }
        if (SlideUI != null)
        {
            SlideUI.gameObject.SetActive(state);
        }  

        //if (col != null)
        //    col.enabled = state;
    }
}
