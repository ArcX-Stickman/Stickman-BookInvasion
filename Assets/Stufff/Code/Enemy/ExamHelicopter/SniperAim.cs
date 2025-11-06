using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperAim : MonoBehaviour
{
    public Transform Player;
    public float RotationSpeed;
    public LineRenderer laser;  
    public float laserWidth = 0.05f; 
    public Color laserColor = Color.red; 
    public Transform AimPoint;
    private Quaternion targetRotation;

    public float laserDelay = 0.5f;  
    private bool isLaserActive = false;

    // Start is called before the first frame update
    void Start()
    {
        targetRotation = transform.rotation;

        if (laser != null)
        {
            laser.startWidth = laserWidth;
            laser.endWidth = laserWidth;
            laser.startColor = laserColor;
            laser.endColor = laserColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Player != null)
        {
            Vector3 direction = Player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, Mathf.Clamp(angle, -60f, 60f));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
            if (!isLaserActive)
            {
                StartCoroutine(UpdateLaserWithDelay(direction));
            }
        }
    }

    IEnumerator UpdateLaserWithDelay(Vector3 direction)
    {
        isLaserActive = true;

        yield return new WaitForSeconds(laserDelay);

        if (laser != null)
        {
            laser.SetPosition(0, AimPoint.position);
            laser.SetPosition(1, Player.position);
        }

        isLaserActive = false;  
    }
}
