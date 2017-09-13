using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LoadLevelTrigger : MonoBehaviour
{
    public LevelManager lm;

    private void OnTriggerEnter(Collider other) { lm.LoadNextLevel(); }
}
