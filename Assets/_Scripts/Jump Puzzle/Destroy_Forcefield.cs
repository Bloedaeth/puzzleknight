using UnityEngine;

public class Destroy_Forcefield : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Destroy(GameObject.FindGameObjectWithTag("ForceField"));
            enabled = false;
        }
    }
}


