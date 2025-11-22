using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    public bool isRight;
    public Transform Left;
    public Transform Right;
    public float speed;
    public Health PlayerHP;
    public Movement StickManager;
    public float DMG;
    public bool isOG;
    public float Distance;
    public AudioSource AudioP;
    public AudioClip Hit;
    void Update()
    {
        if(Left != null && Right != null)
        {
            Vector3 target = isRight ? Right.position : Left.position;
            float distance = Vector2.Distance(transform.position, target);
            target.y = transform.position.y;
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
            if (distance <= 45 && !isOG)
            {
                Destroy(gameObject);
            }
            Distance = distance;
        }
      


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioP.PlayOneShot(Hit);
            StickManager.StickAnim.CrossFade("Stick_Mega_Hurt", 0, 0);
            StickManager.CameraScript.StartCoroutine(StickManager.CameraScript.ShakeCam(3.5f, 0.15f));
            PlayerHP.PlayerHP -= DMG;
            Destroy(gameObject);
        }
    }

    public IEnumerator DestroyIn_Sum_Time()
    {
        int timer = 0;
        int MaxTimer = 10;
        while (timer > MaxTimer)
        {
            timer += 1;


        }
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
