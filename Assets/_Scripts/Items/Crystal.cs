using UnityEngine;

public class Crystal : MonoBehaviour
{
    protected void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
}
