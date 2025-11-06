using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampManager : MonoBehaviour
{
    public AudioSource AudioP;
    public AudioClip Click;
    public Animator SkillTreeAnim;
    public GameObject SkillTree;
    public Animator CampUI;
    private void Start()
    {
        SkillTreeAnim.SetBool("Active",false);
        SkillTree.gameObject.SetActive(false);
        CampUI.SetBool("Active", true);

    }
    public void Upgrade()
    {
        AudioP.PlayOneShot(Click);
        SkillTree.gameObject.SetActive(true);
        SkillTreeAnim.SetBool("Active", true);
        CampUI.SetBool("Active",false );
    }

    public void CloseSkillTree()
    {
        AudioP.PlayOneShot(Click);
        SkillTreeAnim.SetBool("Active", false);
        CampUI.SetBool("Active", true);
    }
}
