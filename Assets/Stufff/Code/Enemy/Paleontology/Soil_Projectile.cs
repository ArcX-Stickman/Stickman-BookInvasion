using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Soil_Projectile : MonoBehaviour
{
    public Movement StickmanManager;
    public Health StickHealth;
    public bool AlrTouched;
    public float DMG;
    public EnemyAI EnemyReciver;
    public float angle;
    public float X;
    public float Y;
    public Transform StickmanPOS;
    public bool isOG;
    public BoxCollider2D BxCd;
    public BoxCollider2D StickBxCd;
    public void GoToPlayer(float angle1,float X1, float Y1)
    {
       
        angle = angle1;
        X = X1;
        Y = Y1;
         //
    }
    public void Update()
    {
        if (BxCd.IsTouching(StickBxCd) && !StickmanManager.BackFlip && !StickmanManager.isDashing)
        {
            if (!AlrTouched && !isOG && EnemyReciver != null)
            {
                EnemyReciver.Meteo_Stun();
                AlrTouched = true;
                StickHealth.PlayerHP -= DMG;
            }
        }
        Vector3 Direc = new Vector3(X, Y, 0f) * Time.deltaTime;
        transform.position -= Direc;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            ObjectPooler.ReturnObjectToPool(gameObject);
        }



    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ObjectPooler.ReturnObjectToPool(gameObject);
        }


    }

}

