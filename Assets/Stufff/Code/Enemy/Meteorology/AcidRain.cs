using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidRain : MonoBehaviour
{

    public GameObject Rain;

    public Transform[] RainAreas;
    public Animator Animator;
    public PauseManager PauseManager;
    public float RainCD;
    private float LastRainDrop;
    public float Distance;
    public float MinDis;
    public Transform Player;
    public float SecondsBeforeDeletion;
    public bool isOG;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private bool StartedDespawn;
    // Update is called once per frame
    void Update()
    {
        if (!StartedDespawn && !isOG)
        {
            StartedDespawn = true;
            StartCoroutine(DespawnTimer());
        }
        float distance = Vector3.Distance(transform.position, Player.transform.position);
        Distance = distance;

        if(PauseManager.isPaused == false && Distance <= MinDis)
        {
            RainFunction();
        }
        
    }
    private void OnDestroy()
    {
        Animator.SetTrigger("1");
    }
    public IEnumerator DespawnTimer()
    {
        yield return new WaitForSecondsRealtime(SecondsBeforeDeletion);
        Animator.SetTrigger("1");
        yield return new WaitForSecondsRealtime(1);
        ObjectPooler.ReturnObjectToPool(gameObject);
        yield return null;
    }

    public void RainFunction()
    {
        if(Time.time >= RainCD + LastRainDrop)
        {
            LastRainDrop = Time.time;
            Transform randomRainArea = RainAreas[UnityEngine.Random.Range(0, RainAreas.Length)];
            //GameObject RainDrops = Instantiate(Rain, randomRainArea.position, Quaternion.identity);
            GameObject RainDrops = ObjectPooler.SpawnObj(Rain, randomRainArea.position, Quaternion.identity, ObjectPooler.PoolType.GamObjects);
        }
       
    }

}
