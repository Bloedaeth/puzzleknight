using System.Collections.Generic;
using UnityEngine;

public class TimeFreezeCollider : MonoBehaviour
{
    private List<GameObject> frozenObjects = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if(enemy == null)
            return;

        enemy.SlowedTime = true;
        frozenObjects.Add(enemy.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if(enemy == null)
            return;
        
        enemy.SlowedTime = false;
        frozenObjects.Remove(enemy.gameObject);
    }

    public void EndFreeze()
    {
        for(int i = 0; i < frozenObjects.Count; ++i)
            frozenObjects[i].GetComponent<Enemy>().SlowedTime = false;

        frozenObjects.Clear();
        gameObject.SetActive(false);
    }
}
