using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboveScientist : MonoBehaviour
{

    public Transform StickmanPOS;
    public float MinDIS;
    public float AttackCD;
    private float LastAttack;
    public BeakerProjectile Beaker;
    public Animator EnemAnim;
    public float Distance;
    public float beakerSpeed;
    public Transform Aimpoint;
    public float BeakerDMG;
    public GameObject[] Eyes;
    public Animator Animator;
    private void Start()
    {
        GameObject Fossil = Eyes[UnityEngine.Random.Range(0, Eyes.Length)];
        Fossil.gameObject.SetActive(true);

    }
    // Update is called once per frame
    void Update()
    {
        // Calc the dis
        float dis = Vector2.Distance(transform.position, StickmanPOS.position);
        Distance = dis;
        if (Time.time >= LastAttack + AttackCD)
        {
            LastAttack = Time.time;
            EnemAnim.SetTrigger("Throw");
        }
    }
    public void YoThrowThrowing()
    {
        // We gotta find the player
        // Projectile motion vy = sqrt (2gh)

        Vector2 direction = (StickmanPOS.position - Aimpoint.position).normalized;
        Vector2 targetPosition = new Vector2(StickmanPOS.position.x, transform.position.y);


        if (targetPosition.x < transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);

        }
        else if (targetPosition.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        GameObject projectile = Instantiate(Beaker.gameObject, Aimpoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        BeakerProjectile beaker = projectile.GetComponent<BeakerProjectile>();
        beaker.DMG = BeakerDMG;
        float velocityX = direction.x * beakerSpeed;
        float arcHeight = 0.9f;
        float velocityY = Mathf.Sqrt(2 * Mathf.Abs(Physics2D.gravity.y) * arcHeight);
        rb.velocity = new Vector2(velocityX, velocityY);
        rb.AddTorque(570f);
        beaker.isOG = false;


    }
}
