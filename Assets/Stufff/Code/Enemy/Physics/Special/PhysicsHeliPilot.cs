using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PhysicsHeliPilot : MonoBehaviour
{
    public GameObject Pack;
    public Exam Exam;
    private bool ThrewPack;
    private float PackSpeed = 24;
    public Animator animator;
    private SpriteRenderer spriteRenderer;
    public Transform PlaceToStand;
    public bool NeedToThrow;
    public Exam_Parachute Exam_Parachute;
    public bool Jump;
    void Update()
    {
        transform.position = PlaceToStand.position;
        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        if(Exam != null)
        {
            if (Exam.Health <= Exam.HealthNeededForPhase3 && !ThrewPack && NeedToThrow)
            {
                ThrewPack = true;
                StartCoroutine(ThrowPack());
                animator.enabled = false;
                spriteRenderer.enabled = false;
            }
            else
            {
                animator.enabled = true;
                spriteRenderer.enabled = true;

            }
        }
        

        if(Exam_Parachute.gameObject.activeSelf)
        {
            animator.SetTrigger("Boom");
        }
        if (Jump)
        {
            StartCoroutine(JumpToParachute());
        }

    }

    public void JumpAnim()
    {
        Jump = true;
    }
    public IEnumerator JumpToParachute()
    {
        float Dis = Vector2.Distance(transform.position, Exam_Parachute.transform.position);
        while( Dis >= 6)
        {
            Dis = Vector2.Distance(transform.position, Exam_Parachute.transform.position);
            transform.position = Vector2.MoveTowards(transform.transform.position, Exam_Parachute.transform.position, 110 * Time.deltaTime);
            Exam_Parachute.Physics.gameObject.SetActive(false);

            yield return null;
        
        }
        Exam_Parachute.Physics.gameObject.SetActive(true);
        Destroy(gameObject);


        yield return null;
    }

    public IEnumerator ThrowPack()
    {
        float Distance = (Pack.transform.position.y - Exam.transform.position.y);
        while(Distance >= 5)
        {
            Pack.transform.position = Vector2.MoveTowards(Pack.transform.position, Exam.transform.position, PackSpeed * Time.deltaTime);
            Distance = Vector2.Distance(Pack.transform.position, Exam.transform.position);
            
            yield return null;

        }
        Exam.CaughtPara = true;
        Pack.gameObject.SetActive(false);
        Exam.ExamAnim.SetTrigger("Para");

        Debug.Log("ExamCaught");
    }
}
