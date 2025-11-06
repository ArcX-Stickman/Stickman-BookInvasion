using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{

    public Animator LeafAnim;


    public float speed = 4;


    public Transform Player;







    // Start is called before the first frame update
    void Start()
    {
        LeafAnim.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void GoToPlayer()
    {
        Vector3 direction = (Player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }
}
