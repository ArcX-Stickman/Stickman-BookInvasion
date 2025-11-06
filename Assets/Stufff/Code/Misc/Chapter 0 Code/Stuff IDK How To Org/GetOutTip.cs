using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetOutTip : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public Animator Anim;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Text.text == "When Hit Press [T] To Heal")
        {
            StartCoroutine(Waitingtogetout());
        }
    }


    public IEnumerator Waitingtogetout()
    {
        yield return new WaitForSecondsRealtime(4);
        Anim.SetTrigger("GoBack");
    }

}
