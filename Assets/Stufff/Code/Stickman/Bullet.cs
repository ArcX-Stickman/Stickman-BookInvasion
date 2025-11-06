using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    private Vector2 direction;
    public Health PlayerManager;
    public Biology Bio_Manager;
    public bool Arc;
    public bool IsGone;
    public bool isOnline;
    public Transform StickPOS;
    public bool isEnemyCounter;
    
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }

    private void Start()
    {
        IsGone = false;
        if(StickPOS.localScale.x >= 0)
        {
            // Thats jus the scale i used 
            transform.localScale = new Vector3(0.143525f, 0.1641267f, 1);

        }
        else
        {
            transform.localScale = new Vector3(-0.143525f, 0.1641267f, 1);

        }
    }

    private void Update()
    {
        // Apply motion blur
       
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
        if (collision.CompareTag("Book"))
        {
            if (isEnemyCounter)
            {
                PlayerManager.MegaCharge += 3.5f;
            }
            IsGone = true;
            //Destroy(gameObject);
            ObjectPooler.ReturnObjectToPool(gameObject);
            isEnemyCounter = false;



            //if (Arc == true && PlayerManager.BiologyBoss == true)
            //{
            // legacy boss used to be the OG Ecology but honnestly bad boss fight
            //    Bio_Manager.VineWhip();
            //}
        }

        if (collision.CompareTag("Ground") || collision.CompareTag("Door"))
        {
            IsGone = true;
            //Destroy(gameObject);
            ObjectPooler.ReturnObjectToPool(gameObject);
            isEnemyCounter = false;

        }

        if (collision.CompareTag("GunKiller"))
        {
            IsGone = true;
            //Destroy(gameObject);
            ObjectPooler.ReturnObjectToPool(gameObject);
            isEnemyCounter = false;

        }
    }
}
