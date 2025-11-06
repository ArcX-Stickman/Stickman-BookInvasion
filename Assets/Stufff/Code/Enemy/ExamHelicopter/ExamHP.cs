using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExamHP : MonoBehaviour
{

    public Slider Slider;
    public Helicopter Exam;

    // Start is called before the first frame update
    void Start()
    {
        Slider.maxValue = Exam.ExamHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Slider.value = Exam.ExamHealth;

    }
}
