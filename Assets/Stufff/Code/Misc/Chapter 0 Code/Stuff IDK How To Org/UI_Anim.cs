using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UI_Anim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    public float scaleFactor = 1.2f;  
    public float animationSpeed = 0.2f; 

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines(); 
        StartCoroutine(ScaleButton(originalScale * scaleFactor));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleButton(originalScale));
    }

    IEnumerator ScaleButton(Vector3 targetScale)
    {
        float elapsedTime = 0;
        Vector3 currentScale = transform.localScale;

        while (elapsedTime < animationSpeed)
        {
            transform.localScale = Vector3.Lerp(currentScale, targetScale, elapsedTime / animationSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale; 


    }

}
