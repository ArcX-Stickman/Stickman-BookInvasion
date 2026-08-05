using UnityEngine;
using UnityEngineInternal;

public class ChangeTransform : MonoBehaviour
{
    public float X;
    public float Y;
    public float Z_Rotation;
    public float Speed;

    public bool OnlyX;
    public bool OnlyY;
    public bool OnlyZ_ROT;
    public bool Frozen;

    void Update()
    {
        if (Frozen) return;
        if (OnlyZ_ROT)
        {
            Vector3 rotation = transform.eulerAngles;

            rotation.z = Mathf.MoveTowardsAngle(
                rotation.z,
                Z_Rotation,
                Speed * Time.deltaTime);

            transform.eulerAngles = rotation;
        }
    }
}