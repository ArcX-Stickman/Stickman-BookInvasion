using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper_Code : MonoBehaviour
{
    public Helicopter Helicopter;
    public Transform AimPoint;
    public GameObject[] GunBullet;
    public float Cooldown;
    public float LastShot;
    public Animator Exam_Anim;
    public PauseManager pause;
    public Transform StickPOS;
    private float HypeLevel = 0;
    // Update is called once per frame
    void Update()
    {
        if(Time.time >= LastShot + Cooldown && !PauseManager.isPaused)
        {
            SniperShoot();
            LastShot = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.F10))
        {
            SniperShoot();
        }
    }
    public void SniperShoot()
    {
        Exam_Anim.SetTrigger("Shoot");
    }
    public void ShootABullet()
    {
        int Hype = 0;
        if(HypeLevel >= 1)
        {
            Hype = 1;
        }
        else
        {
            Hype = 0;
        }
        int rnd = UnityEngine.Random.Range(0,GunBullet.Length);
        GameObject bullet = Instantiate(GunBullet[rnd], AimPoint.position, transform.rotation);
        SniperBullet sniperBullet = bullet.GetComponent<SniperBullet>();

        
        Vector2 shootDirection = (StickPOS.position - AimPoint.position).normalized;
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        sniperBullet.NotOG = true;
        sniperBullet.SetDirection(shootDirection, angle);

       


    }




}
