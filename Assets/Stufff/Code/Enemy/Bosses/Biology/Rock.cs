using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock: MonoBehaviour
{

    public Health PlayerHP;

    public float DMG;

    public Transform Stickman;

    public GameObject FX;

    public Biology BioManager;

    public AudioSource AudioP;

    public AudioClip RockSFX;

    public bool hitPlayer;

    public Rigidbody2D rb;

    public bool TouchedGround;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        hitPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        float moveSpeed = speed * Time.deltaTime;
        transform.Translate(moveSpeed, 0, 0);
    }


    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            if (!TouchedGround)
            {
                TouchedGround = true;
                BioManager.Hurt.gameObject.SetActive(true);
                GameObject effect = Instantiate(FX, Stickman.position, Quaternion.identity);
                if (!hitPlayer)
                {
                    hitPlayer = true;
                    PlayerHP.PlayerHP = PlayerHP.PlayerHP - DMG;
                }

                AudioP.PlayOneShot(RockSFX);
                StartCoroutine(DestroyAfterWhile());
            }
            


        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            TouchedGround = true;
            StartCoroutine(DestroyAfterWhile());
        }



    }

    private IEnumerator DestroyAfterWhile()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
