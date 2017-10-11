using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LoadLevelTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Player>())
            FindObjectOfType<LevelManager>().LoadNextLevelAsync();
    }
}
