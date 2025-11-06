using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TiedUpStickman : MonoBehaviour
{
    public GameObject Player;
    public Movement Movement;
    public AudioClip BreakSFX;
    public AudioSource AudioP;
    public CameraScript Cam;
    private bool Assign;
    public GameObject FX;

    private void Start()
    {
        if (Movement.HasBrokeOutOfRope)
        {
            Destroy(gameObject);
        }
       
        
        
    }
    void Update()
    {
        if (!Movement.HasBrokeOutOfRope &&! Assign)
        {
           Assign = true;
            Cam.Stick = Movement.gameObject.transform;
            Cam.Stick = gameObject.transform;
            StartCoroutine(GiveALittleDelay());
        }
        if (Movement.HasBrokeOutOfRope)
        {
            Destroy(gameObject);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(FX,transform.position,Quaternion.identity);
            Cam.Stick = Player.transform;
            Player.transform.position = transform.position;
            Movement.enabled = true;
            Movement.HasGun = true;
            Movement.Checkpoint = 551;
            Movement.HasBrokeOutOfRope = true;
            Movement.SaveProgress();
            Destroy(gameObject);

        }
    }

    private IEnumerator GiveALittleDelay()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        Movement.enabled = false;
    }
}
