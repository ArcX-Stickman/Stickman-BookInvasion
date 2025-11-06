using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormCloud : MonoBehaviour
{
    public EnemyAI EnemyReciever;
    public float DMG;
    public Animator CloudAnim;
    public Movement PlayerManager;
    public Health PlayerHP;
    public BoxCollider2D ThunderCollider;
    public BoxCollider2D StickManBxcd;
    public bool IsOG;
    public bool AlrTouch;
    public AudioSource AudioP;
    public AudioClip Thunder;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ThunderCollider.IsTouching(StickManBxcd) && EnemyReciever != null && !IsOG && !AlrTouch)
        {
            AlrTouch = true;
            PlayerHP.PlayerHP -= DMG;
            EnemyReciever.Meteo_Stun();
        }
    }
    public void SFX()
    {
        if (!IsOG)
        {
            AudioP.PlayOneShot(Thunder);

        }


    }
    public void DestroyIT()
    {
        if (!IsOG)
        {
            ObjectPooler.ReturnObjectToPool(gameObject);
        }
    }
}
