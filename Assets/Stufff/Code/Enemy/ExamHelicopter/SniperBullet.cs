using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SniperBullet : MonoBehaviour
{
    public Health StickHP;
    private Transform player;
    private Vector2 direction;
    public float speed = 20f;
    public float turnSpeed = 5f;
    public float LifeTime = 5f;
    public bool NotOG;
    public Movement PlayerManager;
    public bool Touched;
    public bool UsedForCounter;
    public float DMG = 1f;
    public float Distance;
    public float MinDIS = 1f;
    public GameObject Boom;
    public AudioSource AudioP;
    public AudioClip BoomSFX;
    private bool AlrFX;
    private TurnOffRigidBodyIfPlayerFar Rb_Checker;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (direction == Vector2.zero)
            direction = transform.right;

        rb.velocity = direction * speed;

        if (NotOG)
            Destroy(gameObject, LifeTime);
    }

    public void SetDirection(Vector2 newDirection, float angle)
    {
        direction = newDirection.normalized;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void FixedUpdate()
    {
        if (Rb_Checker == null)
            Rb_Checker = GetComponent<TurnOffRigidBodyIfPlayerFar>();

        if (Rb_Checker != null && Rb_Checker.Distance < MinDIS)
            return;

        if (player == null || PlayerManager == null)
            return;

        Distance = Vector2.Distance(transform.position, PlayerManager.transform.position);

        // Homing behavior
        Vector2 toPlayer = ((Vector2)player.position - rb.position).normalized;
        Vector2 newDir = Vector2.Lerp(rb.velocity.normalized, toPlayer, turnSpeed * Time.fixedDeltaTime);
        rb.velocity = newDir.normalized * speed;

        float angle = Mathf.Atan2(newDir.y, newDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Catch or damage logic
        if (Distance <= MinDIS && PlayerManager.CanCatch)
        {
            if (UsedForCounter)
            {
                StickHP.PlayerHP += 1;
                PlayerManager.CanCatch = false;
                PlayerManager.HasCaught = true;
                PlayerManager.StickAnim.SetTrigger("Hurt");
            }
            else
            {
                PlayerManager.StickAnim.SetTrigger("Hurt");
                StickHP.PlayerHP -= DMG + (DMG / 2);
            }

            Touched = true;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Touched) return;

        if (collision.CompareTag("Player"))
        {
            PlayerManager.TimeManager.FreezeTime(0.01f);
            PlayerManager.CameraScript.StartCoroutine(
                PlayerManager.CameraScript.ShakeCamGroundFeeling(65.5f, 0.5f));

            if (!AlrFX)
            {
                AudioP.PlayOneShot(BoomSFX);
                AlrFX = true;
            }

            Instantiate(Boom, transform.position, Quaternion.identity);

            if (StickHP.PlayerHP > 1)
            {
                if (PlayerManager.CanCatch)
                    StickHP.PlayerHP -= DMG + (DMG / 2);
                else if (!PlayerManager.isSliding && !PlayerManager.BackFlip && !PlayerManager.isDashing)
                {
                    StickHP.PlayerHP -= DMG;
                    StickHP.StickManager.FreeezeAnimation_KEY();
                }
                else
                {
                    // Idk wut to put here Sup :)
                }
            }
            else
            {
                StickHP.PlayerHP = 0;
            }

            Touched = true;
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Ground"))
        {
            PlayerManager.TimeManager.FreezeTime(0.01f);
            PlayerManager.CameraScript.StartCoroutine(
                PlayerManager.CameraScript.ShakeCamGroundFeeling(45.5f, 0.8f));

            Instantiate(Boom, transform.position, Quaternion.identity);

            if (!AlrFX)
            {
                AudioP.PlayOneShot(BoomSFX);
                AlrFX = true;
            }

            Touched = true;
            Destroy(gameObject);
        }
    }
}
