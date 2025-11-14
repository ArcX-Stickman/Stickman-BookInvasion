using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Exam_Parachute : MonoBehaviour
{
    public Transform EndPoint;
    public GameObject Physics;
    public Movement Stickman;
    public float speed;
    public float PlayerDistance;
    public float MinDis_PlayerDash;
    private bool FinishedDash;
    public Exam ExamChecker;
    public Helicopter Helicopter;
    public Transform StickDes;
    void Update()
    {
        PlayerDistance = Vector2.Distance(transform.position,Stickman.transform.position);
        if (gameObject.activeSelf)
        {
            GlideManager();
        }
    }


    public void GlideManager() 
    {
        transform.position = Vector2.MoveTowards(transform.position,EndPoint.position,speed * Time.deltaTime);
        if(PlayerDistance >= MinDis_PlayerDash)
        {
            if (!FinishedDash && ExamChecker == null)
            {

                StartCoroutine(StickManagerDash());
            }
        }
    
    }

    public IEnumerator StickManagerDash()
    {
        if (FinishedDash) yield break; // safety guard
        FinishedDash = true;
        Helicopter.ForcedSpeed = true;
        Helicopter.HeliAnim.enabled = false;
        Time.timeScale = 0.99f;
        float Distance = Vector2.Distance(transform.position,Stickman.transform.position);
        while (Distance <= MinDis_PlayerDash) 
        { 
            yield return null;
        }
        Helicopter.HeliAnim.enabled = true;
        Helicopter.ExplosionTime = true;
        yield return new WaitForSecondsRealtime(0.1f);
        Stickman.StickAnim.SetBool("Dash", false);
        Stickman.ResetAllAnimationStates();
        Debug.Log("StickManagerDash completed cleanly.");
    }

}
