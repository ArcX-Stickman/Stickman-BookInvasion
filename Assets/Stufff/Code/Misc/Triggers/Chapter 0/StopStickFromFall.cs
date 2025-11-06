using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopStickFromFall : MonoBehaviour
{
    public Transform Stickman; // Reference to the Stickman transform
    private float resetYPosition = -2.338124f; // Y position to reset to

    // Update is called once per frame
    void Update()
    {
        // Check for jump input (spacebar) and if Stickman is below the reset threshold
        if (Stickman.position.y < resetYPosition)
        {
            FallStick();
        }
        
        if (Input.anyKey && Stickman.position.y < resetYPosition)
        {
            FallStick();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FallStick();
        }
    }


    private IEnumerator FallStick()
    {
        // Reset the Y position of the Stickman
        Vector3 newPOS = Stickman.position;
        newPOS.y = resetYPosition;
        Stickman.position = newPOS;

        yield return new WaitForSeconds(0.000000000000001f);
    }
}
