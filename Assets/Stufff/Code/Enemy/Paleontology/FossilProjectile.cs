using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FossilProjectile : MonoBehaviour
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
    public bool HitPlayer;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !AudioS.isPlaying)
        {
            AudioS.PlayOneShot(Boom);
            HitPlayer = true;
            StartCoroutine(DestroyIt());
        }


        if (collision.gameObject.CompareTag("ArcBullet"))
        {
            PlayerHP.PlayerHP = PlayerHP.PlayerHP + 14f;
            PlayerHP.MegaCharge = PlayerHP.MegaCharge + 5f;
            AudioS.PlayOneShot(Boom);
            GameObject effect = Instantiate(FX, SulfurPOS.position, Quaternion.identity);
            Destroy(gameObject);

        }


        if (collision.gameObject.CompareTag("Bullet"))
        {
            PlayerHP.PlayerHP = PlayerHP.PlayerHP + 5f;
            PlayerHP.MegaCharge = PlayerHP.MegaCharge + 1.2f;

            AudioS.PlayOneShot(Boom);
            GameObject effect = Instantiate(FX, SulfurPOS.position, Quaternion.identity);
            Destroy(gameObject);

        }


        if (collision.gameObject.CompareTag("Player"))
        {
            if (!HitPlayer && !PlayerThing.BackFlip && !PlayerThing.isDashing)
            {
                HitPlayer = true;
                BioManager.Hurt.gameObject.SetActive(true);
                PlayerHP.PlayerHP = PlayerHP.PlayerHP - DMG;
                PlayerThing.StickAnim.SetTrigger("Hurt");
                AudioS.PlayOneShot(Boom);
                GameObject effect = Instantiate(FX, PlayerPOS.position, Quaternion.identity);
                StartCoroutine(DestroyIt());
            }
            


        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
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
    }





    public IEnumerator DestroyIt()
    {
        yield return new WaitForSeconds(4.3f);
        AudioS.PlayOneShot(Boom);
        GameObject effect = Instantiate(FX, SulfurPOS.position, Quaternion.identity);
        Destroy(gameObject);
    }





    void GoToPlayer()
    {

    }
}
