using UnityEngine;

public class ChangePosition_AnimKey : MonoBehaviour
{
    public Transform Pos;
    public GameObject Obj;
    public bool IgnoreZ;

    public void ChangePos()
    {
        // Safety check to prevent NullReferenceExceptions
        if (Pos == null || Obj == null)
        {
            Debug.LogError("ChangePosition_AnimKey: Please assign 'Pos' and 'Obj' in the Inspector!", this);
            return;
        }

        // Determine Z target based on IgnoreZ toggle
        float targetZ = IgnoreZ ? transform.position.z : Pos.position.z;

        // Apply new position and activate object
        Obj.transform.position = new Vector3(Pos.position.x, Pos.position.y, targetZ);
        Obj.SetActive(true);
    }
}