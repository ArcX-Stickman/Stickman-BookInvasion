using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [Header ("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;
    public bool CanMove;

    [Header("Movement parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Idle Behavior")]
    [SerializeField] private float idleDur;
    private float idleTimer;


    [Header("Enemy Animatior")]
    [SerializeField] private Animator anim;


    private void Awake()
    {
        initScale = enemy.localScale;
    }




    private void OnDisable()
    {
        anim.SetBool("Walk", false);
        anim.SetBool("Run", false);
    }



    private void Update()
    {
        if (CanMove)
        {
            if (movingLeft)
            {

                if (enemy.position.x >= leftEdge.position.x)
                    MoveInDirection(1);

                else
                {
                    //change the way he moving you gotta change the way or you gonna fall
                    DirectionChange();

                }
            }
            else
            {
                if (enemy.position.x <= rightEdge.position.x)
                    MoveInDirection(-1);
                else
                    DirectionChange();



            }
        }
       

        
    }

    private void DirectionChange()
    {
        anim.SetBool("Run", false);
        anim.SetBool("Walk", false);
        idleTimer += Time.deltaTime;

        if(idleTimer> idleDur)
            movingLeft = !movingLeft;



    }




    private void MoveInDirection(int _direction)
    {

        idleTimer = 0;
        anim.SetBool("Run", true);
        anim.SetBool("Walk", true);
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, initScale.y, initScale.z);
        //make the look the right way and ake him look 9000 IQ
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed, enemy.position.y, enemy.position.z);

    }


    







}
