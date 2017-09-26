using UnityEngine;

public class Destroy_Forcefield : MonoBehaviour
{
    private new AudioSource audio;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider o)
    {
        if(o.CompareTag("Player"))
            Destroy(GameObject.FindGameObjectWithTag("ForceField"));

        audio.Play();
    }

    private void OnTriggerExit(Collider o)
    {
        audio.Play();
    }
}


