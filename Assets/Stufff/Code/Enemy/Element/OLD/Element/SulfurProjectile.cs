using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SulfurProjectile : MonoBehaviour
{

    public float DMG = 15;
    public float Lifespan;
    public Health PlayerHP;
    public Transform PlayerPOS;
    public Movement PlayerThing;
    public float speed;
    public Transform ProjectilePOS;
    private Rigidbody2D rb;
    public AudioSource AudioS;
    public AudioClip Boom;
    public GameObject FX;
    public Transform SulfurPOS;
    public Biology BioManager;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            GameObject effect = Instantiate(FX, SulfurPOS.position, Quaternion.identity);
            Destroy(gameObject);
            
        }


        if (collision.CompareTag("ArcBullet"))
        {
            PlayerHP.PlayerHP = PlayerHP.PlayerHP + 3.4f;
            PlayerHP.MegaCharge = PlayerHP.MegaCharge + 0.5f;
            AudioS.PlayOneShot(Boom);
            GameObject effect = Instantiate(FX, SulfurPOS.position, Quaternion.identity);
            Destroy(gameObject);

        }


        if (collision.CompareTag("Bullet"))
        {
            PlayerHP.PlayerHP = PlayerHP.PlayerHP + 0.4f;
            PlayerHP.MegaCharge = PlayerHP.MegaCharge + 0.2f;

            AudioS.PlayOneShot(Boom);
            GameObject effect = Instantiate(FX, SulfurPOS.position, Quaternion.identity);
            Destroy(gameObject);

        }


        if (collision.CompareTag("Player"))
        {
            BioManager.Hurt.gameObject.SetActive(true);
            PlayerHP.PlayerHP = PlayerHP.PlayerHP - DMG;
            PlayerThing.StickAnim.SetTrigger("Hurt");
            AudioS.PlayOneShot(Boom);
            GameObject effect = Instantiate(FX, PlayerPOS.position, Quaternion.identity);
            Destroy(gameObject);

        }
    }





    



    void GoToPlayer()
    {

    }




}
