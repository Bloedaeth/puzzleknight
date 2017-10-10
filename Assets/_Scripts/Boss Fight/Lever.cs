using GameLogging;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class Lever : MonoBehaviour
{
    public Pylon pylon;
    
    private Animation anim;
    private bool active;

    private void Start()
    {
        anim = GetComponent<Animation>();
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
            BuildDebug.Log("Lever deactivated");
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
        BuildDebug.Log("Lever activated");
        active = val;
        anim.Play("Flip Lever");
        anim["Flip Lever"].speed *= -1;
    }
}
