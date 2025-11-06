using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamCounter : MonoBehaviour
{


    public bool AlrDid;
    public Health StickHP;
    public GameObject FX;
    public Transform HeliPOS;
    public Transform PlayerPos;
    public Helicopter ExamManager;
    public float DMG;
    public AudioSource AudioP;
    public AudioClip SFX;
    public Vector3 direction;
    public float turnSpeed;
    public float speed;
    public bool UsedForCounter;
    // Start is called before the first frame update



    public void Update()
    {
        if (HeliPOS != null)
        {
            Vector2 toPlayer = (HeliPOS.position - transform.position).normalized;

            direction = Vector2.Lerp(HeliPOS.position, toPlayer, turnSpeed * Time.deltaTime).normalized;

            direction = new Vector3(direction.x, 1, 0); 

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Book"))
        {
            if (!AlrDid)
            {
                AlrDid = true;
                StickHP.PlayerHP += 1.2f;
                StickHP.MegaCharge += 2.4f;
                GameObject effect = Instantiate(FX, HeliPOS.position, Quaternion.identity);



                ExamManager.ExamHealth -= DMG;
                AudioP.PlayOneShot(SFX);
                Destroy(gameObject);
            }
        }
    }
}
