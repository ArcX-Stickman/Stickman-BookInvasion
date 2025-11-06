using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fox : MonoBehaviour
{
    public Transform Bush;
    private Transform OldBushPOS;
    public float Speed;
    public float Health;
    public Movement PlayerManager;
    public Health StickHP;
    public Transform RaycastPOS;
    private bool AlrAte;
    private bool AlrCalculated;
    private float FoxDirection;
    public float DistanceOfRayCast;
    public LayerMask WhatsABushBoi;
    public float Distance;
    public float MinDIS;
    public Animator FoxAnim;
    public Transform FoxSpawn;
    private bool FinishedEatAnimation;
    public BoxCollider2D FoxBxCd;
    public BoxCollider2D SpawnCollider;
    public bool isOG;
    public AudioSource AudioP;
    public AudioClip GobbleSFX;
    // Start is called before the first frame update
    void Start()
    {
        AlrCalculated = false;
        AlrAte = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Bush != null)
        {
            Vector2 targetPosition = new Vector2(Bush.position.x, transform.position.y);
            if (targetPosition.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1.9734f, 2.2126f, 1);

            }
            else if (targetPosition.x > transform.position.x)
            {
                transform.localScale = new Vector3(1.9734f, 2.2126f, 1);
            }
        }
       
        if (!AlrCalculated &&!isOG)
        {
            CalculateForDaBestTree();
            AlrCalculated = true;
        }
       if(Bush != null && !isOG)
        {
            Distance = Vector2.Distance(transform.position, Bush.position);
            if (Distance >= MinDIS)
            {
                transform.position = Vector2.MoveTowards(transform.position, Bush.position, Speed * Time.deltaTime);

            }
            else
            {
                if (!AlrAte)
                {
                    FoxAnim.CrossFade("Eat", 0, 0);
                    AlrAte = true;
                }
            }
        }
        if (Bush == null && AlrCalculated)
        {
            Debug.Log("I mean we tried");
            Destroy(gameObject);
        }
        if (AlrAte && FinishedEatAnimation && !isOG)
        {
            Bush = FoxSpawn;
            transform.position = Vector2.MoveTowards(transform.position, Bush.position, Speed * Time.deltaTime);
            if (FoxBxCd.IsTouching(SpawnCollider))
            {
                Destroy(gameObject);
            }
        }


    }
    public void CalculateForDaBestTree()
    {
        Vector2 rayDirection = new Vector2(transform.localScale.x, 0f);
        RaycastHit2D[] BushRay = Physics2D.RaycastAll(RaycastPOS.position, rayDirection, DistanceOfRayCast, WhatsABushBoi);
        Debug.DrawRay(RaycastPOS.position, rayDirection * DistanceOfRayCast, Color.red);
        float tempDistance = 1200;
        int DistanceIndex = 0;
        if (BushRay.Length > 1)
        {
            List<BerryBushManager> BerryTrees = new List<BerryBushManager>();
            List<float> Distances = new List<float>();
            List<int> BerryCount = new List<int>();
            foreach (RaycastHit2D hit in BushRay)
            {
                BerryBushManager Tree = hit.collider.gameObject.GetComponent<BerryBushManager>();
                float DistanceCalc = Vector2.Distance(transform.position, Tree.transform.position);
                Distances.Add(DistanceCalc);
                BerryTrees.Add(Tree);

            }
            for (int i = 0; i < Distances.Count; i++)
            {
                if (Distances[i] < tempDistance && BerryTrees[i].BerryCount > 0)
                {
                    DistanceIndex = i;
                    tempDistance = Distances[i];

                }
            }
            Bush = BerryTrees[DistanceIndex].transform;
           

        }
    }
    public void EatBerry()
    {
        AudioP.PlayOneShot(GobbleSFX);
        BerryBushManager CurrentTree = Bush.gameObject.GetComponent<BerryBushManager>();
        CurrentTree.BerryCount -= 1;
        FinishedEatAnimation = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spawn") && !isOG && AlrAte && FinishedEatAnimation)
        {
            Destroy(gameObject);
        }
    }
}
