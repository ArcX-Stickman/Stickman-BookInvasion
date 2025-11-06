using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookFetusHit : MonoBehaviour
{

    public float DMG;
    public Embryology EmbryologyManager;
    public AudioClip SFX;
    public AudioSource AudioP;
    public GameObject FX;
    public Transform EmbryPOS;
    public Health StickHP;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private bool AlrDid;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Book"))
        {
            if (!AlrDid)
            {
                AlrDid = true;
                StickHP.PlayerHP += 1.2f;
                StickHP.MegaCharge += 2.4f;
                GameObject effect = Instantiate(FX, EmbryPOS.position, Quaternion.identity);
                Movement stick = StickHP.gameObject.GetComponent<Movement>();
                //stick.CameraScript.StartCoroutine(stick.CameraScript.ShakeCam(3.1f, 0.1f));
                stick.CameraScript.StartCoroutine(stick.CameraScript.ShakeCamGroundFeeling(35.5f, 0.2f));

                stick.TimeManager.FreezeTime(0.1f);

                AudioP.PlayOneShot(SFX);
                Destroy(gameObject);
            }
        }

        
    }


   



}
