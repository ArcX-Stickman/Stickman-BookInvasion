using System.Collections;
using UnityEngine;

public class BeakerProjectile : MonoBehaviour
{
    public Transform PlayerPOS;
    public Transform BeakerPOS;
    public Health PlayerHP;
    public bool HitPlayer;
    public float DMG;
    public GameObject FX;
    public AudioClip BreakSFX;
    public AudioSource AudioP;
    public bool isLoud;
    public Movement PlayerManager;
    public AudioClip Miss;
    public bool UsedInEmbry;
    public bool Decoy;
    public bool UsedForCounter;
    public bool UsedAsbookBulletCatch;
    public float StartedCD;
    public bool CanCollide;
    public bool WantCollisionCD;
    public bool WantTOFacePlayer;
    public bool WantPerfomance;
    private Rigidbody2D rb;
    public bool isOG;
    public bool WantEXTRA_Performance;
    public float DisForDestoy;
    public float XSTart;
    public float YSTart;
    public bool WantNewVersion;
    public Vector3 Destination;
    public float Angle;
    public float Speed;
    public bool Yo_U_Pooled;
    public float Distance;
    public float MinDisForPerfect;
    public bool PerfectDodge;
    private bool AlrDidCD;
    private bool HasBeenCaught;

    void OnEnable()
    {
        HitPlayer = false;
        CanCollide = !WantCollisionCD;
        AlrDidCD = false;
        HasBeenCaught = false;
        GetComponent<Collider2D>().enabled = true;
    }

    void Start()
    {
        XSTart = transform.localScale.x;
        YSTart = transform.localScale.y;

        if (WantTOFacePlayer)
        {
            float facing = PlayerPOS.position.x < transform.position.x ? 1 : -1;
            transform.localScale = new Vector3(facing * Mathf.Abs(XSTart), YSTart, 1);
        }
    }

    void Update()
    {
        if (WantNewVersion)
        {
            float VelocityX = Mathf.Cos(Angle) * Speed;
            float VelocityY = Mathf.Sin(Angle) * Speed;
            Vector3 move = new Vector3(VelocityX, VelocityY, 0f) * Time.deltaTime;
            transform.position -= move;
        }

        if (isLoud)
        {
            if (!AudioP) AudioP = GetComponent<AudioSource>();
            AudioP.volume = 1.0f;
            AudioP.mute = false;
        }

        if (WantPerfomance)
        {
            float Distance = Vector2.Distance(transform.position, PlayerPOS.position);
            if (!rb) rb = GetComponent<Rigidbody2D>();
            rb.simulated = Distance <= 100;

            if (WantEXTRA_Performance && Distance >= DisForDestoy && !isOG)
            {
                ReturnToPool();
            }
        }

        if (!PerfectDodge)
        {
            Distance = Vector2.Distance(transform.position, PlayerPOS.position);

            if (PlayerManager.BackFlip
                && Distance <= MinDisForPerfect
                && Time.time - PlayerManager.LastBackflipTime <= 0.1f) // window fore perfect dodging
            {
                PerfectDodge = true;
                PlayerManager.PerfectDodgeCount += 1;
                AudioP.PlayOneShot(PlayerManager.PerfectDodge);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (WantCollisionCD && !AlrDidCD)
        {
            AlrDidCD = true;
            StartCoroutine(WaitToGetCollision());
            return;
        }

        CanCollide = true;

        if (HasBeenCaught || !CanCollide)
            return;

        if (collision.CompareTag("Player"))
        {
            HandlePlayerCatch();
        }
        else if (collision.CompareTag("StickCatch") && UsedInEmbry && UsedForCounter)
        {
            HitPlayer = true;
            HasBeenCaught = true;
            PlayerManager.HasCaught = true;
            ReturnToPool();
        }
        else if (collision.CompareTag("Ground"))
        {
            HitPlayer = true;
            Instantiate(FX, BeakerPOS.position, Quaternion.identity);
            GetComponent<Collider2D>().enabled = false;
            PlayBreakSound();
            ReturnToPool();
        }
        else if (collision.CompareTag("Bullet"))
        {
            HitPlayer = true;
            PlayerHP.MegaCharge += 0.01f;
            Instantiate(FX, BeakerPOS.position, Quaternion.identity);
            GetComponent<Collider2D>().enabled = false;
            PlayBreakSound();
            ReturnToPool();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnTriggerEnter2D(collision.collider); // reuse logic
    }

    private void HandlePlayerCatch()
    {
        if (PlayerManager.BackFlip  )
        {
            HitPlayer = true;
            PlayerHP.MegaCharge += 2;
            AudioP.PlayOneShot(Miss);
        }

        else if (PlayerManager.CanCatch && UsedForCounter)
        {
            if (Decoy)
            {
                PlayerHP.PlayerHP -= 20;
                PlayerHP.MegaCharge -= 7;
            }
            else
            {
                PlayerHP.MegaCharge += 0.2f;
            }

            HitPlayer = true;
            HasBeenCaught = true;
            PlayerManager.CanCatch = false;
            PlayerManager.HasCaught = true;
            ReturnToPool();
        }
        else if (PlayerManager.isDashing)
        {
            PlayerHP.MegaCharge += 2.3f;
            HitPlayer = true;
            HasBeenCaught = true;
            PlayerManager.CanCatch = false;
            PlayerManager.HasCaught = true;
            AudioP.PlayOneShot(BreakSFX);
            ReturnToPool();
        }
        else
        {
            PlayerHP.PlayerHP -= DMG;
            HitPlayer = true;
            if (!UsedInEmbry)
            {
                PlayerManager.StickAnim.SetTrigger("Hurt");

            }
            if (!UsedInEmbry)
            {
                Instantiate(FX, PlayerPOS.position, Quaternion.identity);

            }
            GetComponent<Collider2D>().enabled = false;
            PlayBreakSound();
            if (UsedInEmbry && !PlayerManager.Dead)
            {
                PlayerManager.StickAnim.CrossFade("Stick_Mega_Hurt", 0, 0);
                PlayerManager.CameraScript.StartCoroutine(PlayerManager.CameraScript.ShakeCam(3.5f, 0.15f));

            }
            if (!UsedInEmbry)
                ReturnToPool();
        }
    }

    private IEnumerator WaitToGetCollision()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        CanCollide = true;
    }

    private void PlayBreakSound()
    {
        if (isLoud)
            AudioSource.PlayClipAtPoint(BreakSFX, transform.position, 2.0f);
        else if (AudioP)
            AudioP.PlayOneShot(BreakSFX);
    }

    private void ReturnToPool()
    {
        if (Yo_U_Pooled)
            gameObject.SetActive(false);
        else
            ObjectPooler.ReturnObjectToPool(gameObject);
    }
}
