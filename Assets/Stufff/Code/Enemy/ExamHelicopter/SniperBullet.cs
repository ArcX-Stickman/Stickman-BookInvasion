using UnityEngine;

public class SniperBullet : MonoBehaviour
{
    public Health StickHP;
    private Transform player;
    private Vector2 direction;
    public float speed;
    public float turnSpeed = 2f;
    public float LifeTime;
    public bool NotOG;
    public Movement PlayerManager;
    public bool Touched;
    public bool UsedForCounter;
    public float DMG;
    public float Distance;
    public float MinDIS;
    public GameObject Boom;
    public AudioSource AudioP;
    public AudioClip BoomSFX;
    private bool AlrFX;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (direction == Vector2.zero)
        {
            direction = transform.right; 
        }
        if (NotOG)
        {
            Destroy(gameObject, LifeTime);

        }
    }

    public void SetDirection(Vector2 newDirection, float angle)
    {
        direction = newDirection.normalized;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        Distance = Vector2.Distance(transform.position, PlayerManager.transform.position); 
        if (player != null)
        {
            
            Vector2 toPlayer = (player.position - transform.position).normalized;

            
            direction = Vector2.Lerp(direction, toPlayer, turnSpeed * Time.deltaTime).normalized;

           
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        // Move forward in the current direction
        if(Distance < 1000)
        {
            transform.Translate(direction * speed * Time.deltaTime, Space.World);

        }

        if (Distance <=MinDIS && PlayerManager.CanCatch )
        {
            if (UsedForCounter)
            {
                StickHP.PlayerHP += 1;
                Touched = true;
                PlayerManager.CanCatch = false;
                PlayerManager.HasCaught = true;
                PlayerManager.StickAnim.SetTrigger("Hurt");
                Destroy(gameObject);
            }
            else
            {
                PlayerManager.StickAnim.SetTrigger("Hurt");
                StickHP.PlayerHP -= DMG + (DMG /2);
                Touched = true;
                Destroy(gameObject);
            }
           
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerManager.TimeManager.FreezeTime(0.01f);
            PlayerManager.CameraScript.StartCoroutine(PlayerManager.CameraScript.ShakeCamGroundFeeling(65.5f, 0.5f));
            AudioP.PlayOneShot(BoomSFX);
             
            GameObject boom = Instantiate(Boom, transform.position, Quaternion.identity);
            if(StickHP.PlayerHP > 1 && !Touched)
            {
                if (PlayerManager.CanCatch)
                {
                    StickHP.PlayerHP -= DMG + (DMG / 2);
                    Destroy(gameObject);
                }

                
                else
                {


                    if (!Touched)
                    {
                        Touched = true;
                        if (!PlayerManager.isSliding && !PlayerManager.BackFlip && !PlayerManager.isDashing && !PlayerManager.BackFlip)
                        {
                            StickHP.PlayerHP -= DMG;
                            StickHP.StickManager.FreeezeAnimation_KEY();

                        }
                        else
                        {
                        }
                        Destroy(gameObject);

                    }
                }
            }
            else
            {
                Touched = true;

                if (!Touched)
                {

                    StickHP.PlayerHP = 0;

                }

            }
            if (!PlayerManager.BackFlip)
            {
              
                Touched = true;

                Destroy(gameObject);

            }
            
        }
        else if (collision.CompareTag("Ground"))
        {
            PlayerManager.TimeManager.FreezeTime(0.01f);
            PlayerManager.CameraScript.StartCoroutine(PlayerManager.CameraScript.ShakeCamGroundFeeling(45.5f, 0.8f));
            Instantiate(Boom, transform.position, Quaternion.identity);

            Touched = true;
            AudioP.PlayOneShot(BoomSFX);

            Destroy(gameObject);
        }
    }
}
