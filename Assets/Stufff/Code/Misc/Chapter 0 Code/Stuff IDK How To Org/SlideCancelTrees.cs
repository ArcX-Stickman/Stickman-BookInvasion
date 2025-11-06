using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlideCancelTrees : MonoBehaviour
{

    public bool FirstTree;
    public GameObject HUDTut;
    public TextMeshProUGUI Text4TUT;
    public Animator HUDAnim;
    public Movement Stickman;
    public BoxCollider2D BoxCollider2D;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {

            if (Stickman.isSliding)
            {
                BoxCollider2D.isTrigger = true;
            }
            else
            {
                BoxCollider2D.isTrigger = false;
            }


            if (FirstTree)
            {
                HUDTut.gameObject.SetActive(true);
                Text4TUT.text = "Press [C] To Slide";
                StartCoroutine(WaitForIt());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        if (FirstTree)
        {
            HUDTut.gameObject.SetActive(true);
            Text4TUT.text = "Press [C] To Slide";
            StartCoroutine(WaitForIt());
        }

        if (Stickman.isSliding)
        {
            BoxCollider2D.isTrigger = true;
        }
        else
        {
            BoxCollider2D.isTrigger = false;
        }
    }

    public IEnumerator WaitForIt()
    {
        yield return new WaitForSecondsRealtime(3);
        HUDAnim.SetTrigger("GoBack");
    }

}
