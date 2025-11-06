using System.Collections;
using UnityEngine;

public class LadderHeli : MonoBehaviour
{
    public Movement Stickman;
    public Exam Exam;
    public Helicopter Helicopter;
    public Transform Pivot;

    public float PlayerForce;
    public float ExamForce;
    public float SwingSpeed = 0.2f;

    private float SwingForce = 0f;

    private bool swingRoutineStarted = false;

    private enum SwingState { Sigma, Stick, Exam }
    private SwingState currentSwingState = SwingState.Sigma;

    private void Update()
    {
        if(Exam != null)
        {
            if (!Stickman.InHeliCineBeatdown && Stickman.HeliFinale)
            {
                if (Stickman.PunchBool)
                {
                    currentSwingState = SwingState.Stick;
                    Stickman.PunchBool = false;
                }
                else if (Exam.isPunch)
                {
                    currentSwingState = SwingState.Exam;
                    Exam.isPunch = false;
                }
                else
                {
                    currentSwingState = SwingState.Sigma;
                }

                // Calculate them torques
                float playerDir = Stickman.transform.position.x - Pivot.position.x;
                float examDir = Exam.transform.position.x - Pivot.position.x;

                float playerTorque = PlayerForce * playerDir;
                float examTorque = ExamForce * examDir;

                SwingForce = Mathf.Lerp(SwingForce, SwingForceTarget(), SwingSpeed);
                transform.rotation = Quaternion.Euler(0, 0, SwingForce);

                // Start coroutine once we dont want dat stackoverflow
                if (!swingRoutineStarted)
                {
                    swingRoutineStarted = true;
                    StartCoroutine(SwingRoutine());
                }
            }
       
        }
    }

    private float SwingForceTarget()
    {
        switch (currentSwingState)
        {
            case SwingState.Stick:
                return 39.842f;
            case SwingState.Exam:
                return -57.91f;
            case SwingState.Sigma:
            default:
                return 0f;
        }
    }

    private IEnumerator SwingRoutine()
    {
        while (true)
        {
            // ts is for make stuff look smooth and gucci
            SwingForce = Mathf.Lerp(SwingForce, SwingForceTarget(), SwingSpeed);

            yield return null; 
        }
    }
}
