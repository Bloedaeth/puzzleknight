using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LoadLevelTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) { FindObjectOfType<LevelManager>().LoadNextLevelAsync(); }
}
