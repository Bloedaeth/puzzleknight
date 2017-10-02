using GameLogging;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
	public static ObjectPooler main;
	public GameObject objectToPool;
	public int numObjectsToPool;
	public bool growList = true;

	private List<GameObject> pooledObjects;

	private void Awake() { main = this; }

	private void Start()
	{
        BuildDebug.Log("Pooling object: " + objectToPool.name + " " + numObjectsToPool + " times", true);
		pooledObjects = new List<GameObject>();
		for(int i = 0; i < numObjectsToPool; i++)
		{
			GameObject obj = Instantiate(objectToPool);
			obj.transform.parent = transform;
			obj.SetActive(false);
			pooledObjects.Add(obj);
		}
	}

	public GameObject GetPooledObject()
    {
        BuildDebug.Log("Getting a pooled object of type " + objectToPool.name, true);
        GameObject any = pooledObjects.FirstOrDefault(obj => !obj.activeInHierarchy);

		if(!any && growList)
        {
            BuildDebug.Log("Growing pooled objects list of type " + objectToPool.name, true);
            GameObject obj = Instantiate(objectToPool);
			obj.transform.parent = transform;
			obj.SetActive(false);
			pooledObjects.Add(obj);
			any = obj;
		}
		return any;
	}
}