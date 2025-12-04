using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class FallingDebris : MonoBehaviour
{
    public Transform EndPosition;
    private Vector3 STartPos;
    public float timeWeWant;
    public float ElapsedTime;
    public float TimeStickOnPlat;

    public AnimationCurve curve;

    private void Start()
    {
        STartPos = transform.position;
    }

    private void Update()
    {
        ElapsedTime += Time.deltaTime;
        float percencomplete = ElapsedTime / timeWeWant;
        Vector2 fake = new Vector2(transform.position.x, EndPosition.position.y);
        transform.position = Vector3.Lerp(STartPos, fake, curve.Evaluate(percencomplete));
    }

}