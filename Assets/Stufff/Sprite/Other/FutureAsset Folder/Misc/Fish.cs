using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public BoxCollider2D LeftSide;
    public BoxCollider2D RightSide;
    private BoxCollider2D FishC;
    private Transform Direc;
    public float Speed;
    public Health Health;
    public float Distance;
    public float DistanceFromPlayer;
    public float MinDis_ForPlayer;
    public Transform PlayerPOS;
    private void Start()
    {
        FishC = GetComponent<BoxCollider2D>();
        int rndNum = UnityEngine.Random.Range(0, 1);
        if (rndNum == 0)
        {
            Direc = RightSide.gameObject.transform;
        }
        else
        {
            Direc = LeftSide.gameObject.transform;
        }

    }

    // Update is called once per frame
    void Update()
    {
        DistanceFromPlayer = Vector2.Distance(transform.position, PlayerPOS.position);
        if(Health.PlayerHP > 80)
        {
            Speed = 25f;
        }
        else if (Health.PlayerHP > 50)
        {
            Speed = 17;
        }
        else
        {
            Speed = 5.3f;

        }
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 TargetPos = new Vector2(Direc.transform.position.x, transform.position.y);
        Distance = Vector2.Distance(transform.position, Direc.position);
        transform.position = Vector2.MoveTowards(currentPos, TargetPos, Speed * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FishR"))
        {
            Direc = LeftSide.gameObject.transform;
            transform.localScale = new Vector3(-8.543515f, 8.92855f, 1);

        }
        if (collision.CompareTag("FishL"))
        {
            Direc = RightSide.gameObject.transform;
            transform.localScale = new Vector3(8.543515f, 8.92855f, 1);


        }
    }
}
