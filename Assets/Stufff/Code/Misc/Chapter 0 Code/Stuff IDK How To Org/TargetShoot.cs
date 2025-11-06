using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetShoot : MonoBehaviour
{
    public GameObject Platforms;
    public AudioSource AudioP;
    public AudioClip TargetSFX;
    public Health StickHP;
    private void Start()
    {
        Platforms.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            ObjectPooler.ReturnObjectToPool(collision.gameObject);  
            Platforms.gameObject.SetActive(true);
            AudioP.PlayOneShot(TargetSFX);
            StickHP.MegaCharge += 5;
            Destroy(gameObject);
        }
    }
}
