using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Lever : MonoBehaviour
{
    //public Material red;
    //public Material green;
    public Pylon pylon;

    //private new Renderer renderer;
    private Animator animator;
    private bool active;

    private void Start()
    {
        //renderer = GetComponent<Renderer>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Player p = other.GetComponent<Player>();
        if(p)
            p.NearInteractableObject = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if(active && Input.GetKeyDown(KeyCode.E))
        {
            //renderer.material = red;
            pylon.SetPylonActive(false);
            SetLeverActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player p = other.GetComponent<Player>();
        if(p)
            p.NearInteractableObject = false;
    }

    public void SetLeverActive(bool val)
    {
        active = val;
        animator.Play("Flip Lever");
        animator.speed *= -1;
        //animation["Flip Lever"].speed *= -1;
        //renderer.material = active ? green : red;
    }
}
