using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterTriggerBossFight : MonoBehaviour
{

    public Movement StickManager;
    public AudioSource SolutionTheme;
    public AudioClip HeliChaseTheme;
    private bool Triggered;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StickManager.Checkpoint = StickManager.ExamHeliID_Checkpoint;
            StickManager.SaveProgress();
            SolutionTheme.clip = HeliChaseTheme;
            SolutionTheme.Play();
            this.enabled = false;
        }
    }
}
