using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsParachute : MonoBehaviour
{

    public GameObject PhysicsFall;
    public PhysicsShot_HeliChaseQTE QTE_Manager;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            QTE_Manager.AudioP.PlayOneShot(QTE_Manager.HurtSFX);
            PhysicsFall.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}
