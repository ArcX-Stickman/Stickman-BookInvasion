using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BulletOnline : NetworkBehaviour
{
    public NetworkVariable<int> playerID = new NetworkVariable<int>();
    private const string PlayerTag = "Player";
    public float speed;
    public float lifetime;
    private Vector2 direction;
    public bool Destroyer;
    public Health PlayerManager;
    public Biology Bio_Manager;
    public bool Arc;
    public bool IsGone;
    public CloneManager CloneManager;
    public bool isOnline;
    public MotionBlur motionBlurHandler;


   
   
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }

    private void Start()
    {
        Invoke("DestroyBullet", lifetime);

    }


    

    private void Update()
    {

        if (!IsOwner)
        {
            gameObject.tag = "OpposingBullet";
        }
        // Movement crap
        Vector2 movement = direction * speed * Time.deltaTime;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, movement.magnitude);

        transform.Translate(movement);
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }


   


    private void OnTriggerEnter2D(Collider2D collision)
    {



        // you guys dont know how much crap i had to go through to get this to work

        if (collision.gameObject.CompareTag(PlayerTag))
        {
            
            StickOnlineManager stick = collision.GetComponent<StickOnlineManager>();
            if (stick.StickID.Value != playerID.Value)
            {
                
                MakeitHurt(stick);
                //stick.GotShot_OhNoo_GetFX();
                //Destroy(gameObject);
            }
            return;
        }

        if (collision.CompareTag("Book") || collision.CompareTag("Ground") || collision.CompareTag("Door") || collision.CompareTag("GunKiller"))
        {
            Destroy(gameObject);
        }

        if (collision.CompareTag("Ground") || collision.CompareTag("Door"))
        {
            IsGone = true;
            Destroy(gameObject);
        }

        if (collision.CompareTag("GunKiller"))
        {
            IsGone = true;
            Destroy(gameObject);
        }
    }


    void MakeitHurt(StickOnlineManager stick)
    {
        if (stick != null)
        {

            Destroy(gameObject);
            stick.GotShot_OhNoo_GetFX();

        }
    }

    void MakeClientKnowUshootSomething(string id)
    {
        Debug.Log(id + "Server knows you shot something");
    }
}
