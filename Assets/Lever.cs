using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public Material red;
    public Material green;
    public Pylon pylon;

    private new Renderer renderer;
    private bool active;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
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
            renderer.material = red;
            pylon.SetPylonActive(false);
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
        renderer.material = active ? green : red;
    }
}
