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
        Helicopter.FinalDestination = Helicopter.StickPOS;
        Helicopter.Place_To_Go = Helicopter.StickPOS;
        // Set up dash state once
        Stickman.OnLadder = false;
        Stickman.HeliFinale = false;
        Stickman.StickAnim.SetBool("Heli", false);
        Stickman.StickAnim.SetBool("Dash", true);
        while (Vector2.Distance(Stickman.transform.position, StickDes.position) > 12.4f)
        {
            Stickman.StickAnim.SetBool("Dash", true);
            Stickman.transform.position = Vector2.MoveTowards(
                Stickman.transform.position,
                StickDes.position,
                (speed + 50) * Time.deltaTime
            );
            yield return null;
        }
        Helicopter.Speed = 130.2f;
        Helicopter.Speed = 130.2f;
        StartCoroutine(Stickman.CameraScript.ZoomIn(25, 2));
        Stickman.TimeManager.SloMO();
        Stickman.StartCoroutine(Stickman.BackFlipMove());
        yield return new WaitForSecondsRealtime(0.3f);
        Helicopter.ExplosionTime = true;
        yield return StartCoroutine(Stickman.CameraScript.ZoomOut(2));
        yield return new WaitForSecondsRealtime(0.1f);
        Stickman.StickAnim.SetBool("Dash", false);
        Stickman.ResetAllAnimationStates();
        Debug.Log("StickManagerDash completed cleanly.");
    }

}
