using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BookDetector : MonoBehaviour
{
    // C# kinda stupid cuz why cant i just say Gameobject[] and then add no u gotta do some crap that look like that huh
    public List<GameObject> BookList = new List<GameObject>();
    public EnemyAI EnemyAI;
    private int EnemysID;


    // Start is called before the first frame update
    void Start()
    {
        EnemysID = EnemyAI.EnemyID;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Book"))
        {
            int ID = collision.GetComponent<EnemyAI>().EnemyID;
            if(ID != EnemysID)
            {
                GameObject book = collision.gameObject;
                AddToList(book);
            }
        }
    }



    public void AddToList(GameObject book)
    {
       
        
    }
}
