using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scientist : MonoBehaviour
{
    public AttackSight AttackSight;
    public float speed;
    public float ATK_CD;
    public Movement PlayerManager;
    public Health StickHP;
    public Transform PlayerPOS;
    public bool TouchingPlayer;
    public float MinDIS;
    public Animator Scientist_Anim;
    public float LastAtk;
    public GameObject[] EyeList;
    public float MeleeDMG;
    public Transform AimPoint;
    public GameObject Beaker;
    public float ProjectileSpeed;
    public RangeSight RangeCheck;
    public AudioSource AudioP;
    public AudioClip PunchSFX;
    public DodgeStreaks SteakManager;
    public AudioClip Miss;
    public float HP;
    public GameObject FX;
    public Transform ScientistPOS;
    public bool isSpecial;
    public bool isAbove;
    public GameObject Alarm;
    public bool Normal;
    public bool SpecialStory;
    public bool IsYapping;
    public AudioSource YappingP;
    public float PublicDistanceForThrowingPerformance;
    public EncounterManagerAfterLab EncounterManager;

    private bool StartYap;
    private bool Clicked = false;

    void Start()
    {
        if (SpecialStory && !StartYap)
        {
            StartYap = true;
            StartCoroutine(WaitforStopYap());
            Scientist_Anim.SetBool("Story", true);
        }

        if (isSpecial)
        {
            PlayerSpotted();
        }

        GameObject Eyes = EyeList[UnityEngine.Random.Range(0, EyeList.Length)];
        Eyes.SetActive(true);
    }

    void Update()
    {
        if (isAbove)
        {
            HandleYappingOnly(); // skip non-rooftop logic
            return;
        }

        PublicDistanceForThrowingPerformance = Vector2.Distance(transform.position, PlayerPOS.position);

        if (Normal)
        {
            PlayerSpotted();
        }
        else if (AttackSight != null && AttackSight.StickmanIsHERE && !IsYapping)
        {
            PlayerSpotted();
            Attack();
        }

        HandleYappingOnly();

        if (Time.time >= LastAtk + ATK_CD)
        {
            LastAtk = Time.time;
            Attack();
        }

        if (HP <= 0)
        {
            if (SpecialStory) EncounterManager.Scientist = null;
            Scientist_Anim.SetTrigger("Dead");
        }

        if (isSpecial && TouchingPlayer)
        {
            Scientist_Anim.SetTrigger("Yo");
        }

        // Flip sprite
        Vector2 targetPosition = new Vector2(PlayerPOS.position.x, transform.position.y);
        transform.localScale = new Vector3(targetPosition.x < transform.position.x ? 1 : -1, 1, 1);
    }

    void HandleYappingOnly()
    {
        if (IsYapping)
        {
            Scientist_Anim.SetBool("Story", true);
            YappingP.enabled = true;
            StartCoroutine(WaitforStopYap());
        }
        else if (SpecialStory)
        {
            YappingP.enabled = false;
            Scientist_Anim.SetBool("Story", false);
        }

        if (AttackSight != null && AttackSight.StickmanIsHERE)
        {
            IsYapping = false;
        }
    }

    public void AlarmClick()
    {
        if (!Clicked)
        {
            Alarm.gameObject.SetActive(true);
            Clicked = true;
        }
    }

    public void PlayerSpotted()
    {
        if (StickHP.PlayerHP <= 0 || PlayerManager.Hiding || IsYapping) return;
        if (Normal && AttackSight != null && !AttackSight.StickmanIsHERE) return;

        float distanceToPlayer = Vector2.Distance(transform.position, PlayerPOS.position);
        if (distanceToPlayer <= MinDIS)
        {
            TouchingPlayer = true;
            Scientist_Anim.SetBool("Walk", false);
        }
        else
        {
            Scientist_Anim.SetBool("Walk", true);
            Vector2 targetPosition = new Vector2(PlayerPOS.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            transform.localScale = new Vector3(targetPosition.x < transform.position.x ? 1 : -1, 1, 1);
        }

        if (TouchingPlayer)
        {
            StickHP.InCombat = true;
        }
    }

    public void HolyYap()
    {
        IsYapping = false;
    }

    public void Attack()
    {
        if (isAbove || IsYapping || StickHP.PlayerHP <= 0) return;

        if (RangeCheck.StickHere && !isSpecial)
        {
            Scientist_Anim.SetTrigger("Fist");
        }
        else
        {
            if (Normal && AttackSight != null && !AttackSight.StickmanIsHERE) return;
            Scientist_Anim.SetTrigger("Throw");
        }
    }

    public void Death()
    {
        StickHP.PlayerHP += 5;
        StickHP.MegaCharge += 0.3f;
        Instantiate(FX, ScientistPOS.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Instantiate(FX, ScientistPOS.position, Quaternion.identity);
            HP -= 1;
            Destroy(collision.gameObject);
            StickHP.MegaCharge += 0.2f;
        }

        if (collision.CompareTag("ArcBullet"))
        {
            Destroy(collision.gameObject);
            Instantiate(FX, ScientistPOS.position, Quaternion.identity);
            HP += 2; // Note: was "-2", changed to "+2" assuming bug?
            StickHP.MegaCharge += 0.6f;
        }

        if (collision.CompareTag("StickMelee"))
        {
            Instantiate(FX, ScientistPOS.position, Quaternion.identity);
            HP -= 0.3f;
        }
    }

    public void MeleeATK()
    {
        if (RangeCheck.StickHere && PlayerManager.Grounded && !IsYapping)
        {
            PlayerManager.StickAnim.SetTrigger("Hurt");
            AudioP.PlayOneShot(PunchSFX);
            StickHP.PlayerHP -= MeleeDMG;
            PlayerManager.MaxSpeed -= UnityEngine.Random.Range(0, 15);
        }
        else
        {
            SteakManager.StreakThing();
            AudioP.PlayOneShot(Miss);
        }
    }

    public IEnumerator WaitforStopYap()
    {
        while (YappingP.isPlaying)
        {
            yield return null;
        }

        IsYapping = false;
    }

    public void ThrowATK()
    {
        if (StickHP.PlayerHP <= 0 || IsYapping || isAbove) return;

        Vector2 direction = (PlayerPOS.position - AimPoint.position).normalized;

        GameObject projectile = Instantiate(Beaker, AimPoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        float enemySpeed = rb.velocity.x;
        float baseThrowSpeed = ProjectileSpeed;
        float velocityX = direction.x * baseThrowSpeed + enemySpeed;
        float arcHeight = 0.5f;
        float velocityY = Mathf.Sqrt(2 * arcHeight * Mathf.Abs(Physics2D.gravity.y));

        rb.velocity = new Vector2(velocityX, velocityY);
        rb.AddTorque(570f);
    }
}
