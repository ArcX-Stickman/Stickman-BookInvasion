using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GerminationSeed_Manager : MonoBehaviour
{
    public Vector3 POS;
    public bool isOG;
    public Transform Y_Axis_Reference;
    private Transform PointOfSpawn;
    public GameObject VineObj;
    public float LifeTime;
    void Update()
    {
        if(!isOG)
        {
            transform.position -= POS * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            SpawnVine();
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Light2D light2D = GetComponent<Light2D>();
            spriteRenderer.enabled = false;
            light2D.enabled = false;
            Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
            rigidbody2D.simulated = false;
            Destroy(gameObject,LifeTime + 1);

        }
    }

    private bool AlrDID;
    public void SpawnVine()
    {
        if (!AlrDID)
        {
            AlrDID = true;
            StartCoroutine(SpawnWithDelay());
        }
        Destroy(gameObject, LifeTime);

    }

    public IEnumerator SpawnWithDelay()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        Vector3 PointOfSpawnT = new Vector3(transform.position.x, Y_Axis_Reference.position.y, 0f);
        GameObject Vine = Instantiate(VineObj, PointOfSpawnT, Quaternion.identity);

    }
}
