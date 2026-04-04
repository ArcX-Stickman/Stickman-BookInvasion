using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellWall : MonoBehaviour
{
    public AudioSource AudioP;
    public AudioClip DMGSFX;
    public float DMG;
    public Health StickHP;
    public Movement Stickman;
    public bool GET_OUT;
    public float lifetime;

    public void Update()
    {
        if (GET_OUT)
        {
            Destroy(gameObject, lifetime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StickHP.PlayerHP -= DMG;
            AudioP.PlayOneShot(DMGSFX);
            Stickman.StickAnim.SetTrigger("Hurt");
        }
    }


}
