using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryBushManager : MonoBehaviour
{
    public int BerryCount;
    public Sprite Berry3;
    public Sprite Berry2;
    public Sprite Berry1;
    public Sprite NoBerrys;
    public SpriteRenderer BerryRenderer;
    public GameObject EmptyTree;
    void Start()
    {
        EmptyTree.gameObject.SetActive(false);
        BerryCount = UnityEngine.Random.Range(2, 3);
    }
    void Update()
    {
        if(BerryCount == 1)
        {
            BerryRenderer.sprite = Berry1;
        }
        else if(BerryCount == 2)
        {
            BerryRenderer.sprite = Berry2;
        }
        else if(BerryCount == 3)
        {
            BerryRenderer.sprite = Berry3;

        }
        else
        {
            Destroy(gameObject);
            EmptyTree.gameObject.SetActive(true);
            BerryRenderer.sprite = NoBerrys;
        }

    }
}
