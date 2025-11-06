using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HealthPackTip : MonoBehaviour
{


    public GameObject tip;
    public Animator TipAnim;
    public TextMeshProUGUI TipText;
    public SpriteRenderer MedPackSprite;
    public Light2D Light2d;
   







    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool AlrDid;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Light2d.enabled = false;
            Color color = MedPackSprite.color;

            color.a = 0;

            MedPackSprite.color = color;
            if (!AlrDid)
            {
                tip.gameObject.SetActive(true);
                AlrDid = true;
                StartCoroutine(WaitForIt());

            }

        }
        
    }


    public IEnumerator WaitForIt()
    {
        
        TipText.text = "To Heal Press [T]";
        yield return new WaitForSecondsRealtime(3);
        TipText.text = "Or Press [MiddleMouse]";
        yield return new WaitForSecondsRealtime(3);
        TipAnim.SetTrigger("GoBack");
        Destroy(gameObject);





    }
}
