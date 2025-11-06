using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffCloud : MonoBehaviour
{

    public GameObject Rain;
    public GameObject[] PercipitationSelection;
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
    public bool AlrPickedRnd;
    // Start is called before the first frame update
    private int index = 0;
    private bool StartedDespawn;

    public float HailPercipitationSpeed;
    public float SleetPercipitationSpeed;
    public float RainPercipitationSpeed;
    // Update is called once per frame
    void Update()
    {
        if(index == 0)
        {
            RainCD = HailPercipitationSpeed;
        }
        if(index == 1)
        {
            Distance = SleetPercipitationSpeed;
        }
        else
        {
            Distance = RainPercipitationSpeed;
        }
        if (!StartedDespawn && !isOG)
        {
            StartedDespawn = true;
            StartCoroutine(DespawnTimer());
        }
        float distance = Vector3.Distance(transform.position, Player.transform.position);
        Distance = distance;

        if (PauseManager.isPaused == false && Distance <= MinDis)
        {
            RainFunction();
        }
        if (!AlrPickedRnd)
        {
            PickIt();
        }

    }
    public void PickIt()
    {
        if (!AlrPickedRnd)
        {
            // This is For Ecology's Boss FIght w/ the Sleet hail and Rain
            int rndIndex = UnityEngine.Random.Range(0, PercipitationSelection.Length);
            index = rndIndex;
            Rain = PercipitationSelection[rndIndex];
            AlrPickedRnd = true;
        }
      
    }
    private void OnDestroy()
    {
        Animator.SetTrigger("1");
    }
    public IEnumerator DespawnTimer()
    {
        if (!isOG)
        {
            yield return new WaitForSecondsRealtime(SecondsBeforeDeletion);
            Animator.SetTrigger("1");
            yield return new WaitForSecondsRealtime(1);
            ObjectPooler.ReturnObjectToPool(gameObject);
            yield return null;
        }
        
    }

    public void RainFunction()
    {
        if (Time.time >= RainCD + LastRainDrop)
        {
            LastRainDrop = Time.time;
            Transform randomRainArea = RainAreas[UnityEngine.Random.Range(0, RainAreas.Length)];
            GameObject RainDrops = Instantiate(Rain, randomRainArea.position, Quaternion.identity);
        }

    }

}
