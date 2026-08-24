using UnityEngine;

public class ActivateObject_Ontrigger_Deac_OnExit : MonoBehaviour
{
    public GameObject ob;

    private void Start()
    {
        if(ob == null)
        {
            ob = gameObject.GetComponent<GameObject>();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && ob != null)
        {
            ob.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && ob != null)
        {
            ob.SetActive(false);
        }
    }
}