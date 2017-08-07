using UnityEngine;

public class MovableObject : MonoBehaviour
{
    public bool BeingMoved;
    private Vector3 pos;

    private void Awake()
    {
        pos = transform.position;
    }

    private void Update()
    {
        if(!BeingMoved)
            transform.position = pos;
        else
            pos = transform.position;
    }
}
