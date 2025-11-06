using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActivateAnObjectWithCollider : MonoBehaviour
{

    public GameObject TheObject;
    public GameObject Enemies;
    public bool wantAlrDid;
    public bool AlrDid;
    public bool WantDeactivate;
    public bool isTut;
    public string txtt;
    public KeyCode KeyCodeE;
    public bool Wantonce;
    public bool Alrdid;
    void Start()
    {
        TheObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !Alrdid)
        {

            if (Wantonce)
            {
                Alrdid = true;
            }
            if(isTut && !WantDeactivate)
            {
                TextMeshProUGUI txt = TheObject.gameObject.GetComponent<TextMeshProUGUI>();
                txt.text = txtt;
                TutorialKeyInput keyCode = TheObject.gameObject.GetComponent<TutorialKeyInput>();
                keyCode.InputS = KeyCodeE;
            }
            if (WantDeactivate)
            {
                TheObject.SetActive(false);

            }
            else
            {
                if (wantAlrDid && !AlrDid)
                {
                    TheObject.SetActive(true);

                    AlrDid = true;
                }
                else if (!wantAlrDid)
                {
                    TheObject.SetActive(true);

                }
            }
            
        }
    }
}
