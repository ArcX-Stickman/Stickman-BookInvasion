using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullAreaOrRender : MonoBehaviour
{



    public GameObject RenderObj;


    private GameObject[] CullList;

    private GameObject[] RenderList;

    private bool Rendering;
    private bool Culled;


    // Update is called once per frame


    private void Start()
    {

        CullList = new GameObject[RenderObj.transform.childCount];

        for (int i = 0; i < RenderObj.transform.childCount; i++)
        {
            CullList[i] = RenderObj.transform.GetChild(i).gameObject;
        }



        RenderList = new GameObject[RenderObj.transform.childCount];

        for (int i = 0; i < RenderObj.transform.childCount; i++)
        {
            RenderList[i] = RenderObj.transform.GetChild(i).gameObject;
        }

        Culling();

    }





    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            if(!Rendering || Culled)
            {
                Render();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Rendering || !Culled)
            {
                Culling();
            }
        }
    }



    private void Culling()
    {
        Culled = true;

        if (CullList.Length > 0)
        {
            foreach (GameObject obj in CullList)
            {
                obj.gameObject.SetActive(false);
            }
        }
        else
        {
            RenderObj.gameObject.SetActive(false);
        }
        
    }

    private void Render()
    {
        Rendering = true;

        if (RenderList.Length > 0)
        {
            foreach (GameObject obj in RenderList)
            {
                obj.gameObject.SetActive(true);
            }
        }
        else
        {
            RenderObj.gameObject.SetActive(false);
        }

    }


}
