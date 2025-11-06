using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform PlayerPOS;
    public float fliptimeY = 0.5f;
    public Coroutine turnCourotine;
    public Movement PlayerManager;
    private bool FacingRight;   
    // Start is called before the first frame update
    void Start()
    {
        PlayerManager = PlayerPOS.GetComponent<Movement>();
        FacingRight = PlayerManager.FacingRight;
    }


    private void LateUpdate()
    {
        transform.position = PlayerPOS.position;
    }
    public void CallTurn()
    {
        turnCourotine = StartCoroutine(FlipY());
    }

    public IEnumerator FlipY()
    {
        float startRotation = transform.rotation.eulerAngles.y;
        float endrotation = FindEndPOS();
        float yrotation = 0;

        float timeelapsed = 0f;
        while(timeelapsed < fliptimeY)
        {
            timeelapsed += Time.deltaTime;
            yrotation = Mathf.Lerp(startRotation, endrotation, (timeelapsed/fliptimeY));
            transform.rotation = Quaternion.Euler(0f, yrotation, 0f);
            yield return null;
        }
    }
    public float FindEndPOS()
    {
        FacingRight = !FacingRight;
        if(FacingRight)
        {
            return 180f;
        }
        else
        {
            return 0f;
        }
    }
}
