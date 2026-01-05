using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class ObjectPooler : MonoBehaviour
{
    public static List<PooledObjectStuff> ObjectPools = new List<PooledObjectStuff>();
    private GameObject objectPoolEmptyHolder;
    private static GameObject particlesystemEmpty;
    private static GameObject gameObjectEmpty;

    public enum PoolType
    {
        Partcles,
        GamObjects, /// how am i this stupid at spelling?!?! now if i change this ima get 67000 errors >:(
        Misc
    }

    public static PoolType PoolingType;
    private void Awake()
    {
        SetUpEmpties();
    }
    private void SetUpEmpties()
    {
        objectPoolEmptyHolder = new GameObject("Pooled Objects");
       
        particlesystemEmpty = new GameObject("Partcles FX");
        particlesystemEmpty.transform.SetParent(objectPoolEmptyHolder.transform);

        gameObjectEmpty = new GameObject("GameObjects");
        gameObjectEmpty.transform.SetParent(gameObjectEmpty.transform);

    }
    public static GameObject SpawnObj(GameObject objectWeWant, Vector3 SpawnPOS, Quaternion spawnRotation, PoolType poolType = PoolType.Misc)
    {
        PooledObjectStuff pool = ObjectPools.Find(p => p.LookupString == objectWeWant.name);

        // YOOOO IT DONT EXIST ;-;
        if(pool == null)
        {
            pool = new PooledObjectStuff() { LookupString = objectWeWant.name };
            ObjectPools.Add(pool);
        }

        //check if sum in the pool
        GameObject SpawnableObj = null;
        foreach(GameObject obj in pool.InactiveObjects)
        {
            if (obj != null)
            {
                SpawnableObj = obj;
                break;
            }
        }

        if(SpawnableObj == null)
        {
            // if nothing then make sum
            GameObject parentObj = SetParent(poolType);
            SpawnableObj = Instantiate(objectWeWant,SpawnPOS,spawnRotation);

            if(parentObj != null)
            {
                SpawnableObj.transform.SetParent(parentObj.transform);
            }

        }

        else
        {
            // if sum avctually inactive get dat boy working again
            SpawnableObj.transform.position = SpawnPOS;
            SpawnableObj.transform.rotation = spawnRotation;
            pool.InactiveObjects.Remove(SpawnableObj);
            SpawnableObj.SetActive(true);
        }

        return SpawnableObj;
    }

    public static void ReturnObjectToPool(GameObject obj)
    {
        string No_more_Clone = obj.name.Substring(0,obj.name.Length-7); // gotta get rid of the (clone) thing at the end
        PooledObjectStuff pool = ObjectPools.Find(p => p.LookupString == No_more_Clone);
        if(pool == null)
        {
            Debug.LogWarning("Ayo Sums Wrong Over Here Dat Object Aint Pooled Buddy Fix This!!"+ obj.name);
        }
        else
        {
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
        }
    }

    private static GameObject SetParent(PoolType poolType)
    {
        switch(poolType)
        {
            case PoolType.Partcles:
                return particlesystemEmpty;
                break;
            case PoolType.GamObjects:
                return gameObjectEmpty;
                break;
            case PoolType.Misc:
                return null;    
            default:
                return null;
        }
    }
  
}

public class PooledObjectStuff
{
    public string LookupString;
    public List<GameObject> InactiveObjects = new List<GameObject>();
}
