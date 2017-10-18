using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class FadeOutLoad : MonoBehaviour
{
    public Transform PathEndPos;

    [SerializeField] private FadeOut fade;

    private void OnTriggerEnter(Collider other)
    {
        Player p = other.GetComponent<Player>();

        if(p)
        {
            p.WalkTo(PathEndPos.position);
            fade.gameObject.SetActive(true);
            StartCoroutine(fade.Fade());
			GetComponentInChildren<CameraChaser>().BeginChase(false);
        }
    }
}
