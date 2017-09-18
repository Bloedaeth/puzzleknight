using UnityEngine;

public class TimeFreezeCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        FreezeObj(other, true);
    }

    private void OnTriggerExit(Collider other)
    {
        FreezeObj(other, false);
    }

    private void FreezeObj(Collider other, bool frozenState)
    {
        IFreezable obj = other.GetComponent<IFreezable>();
        if(obj == null)
            return;
        Debug.Log(other.name + ": " + frozenState);
        obj.SlowedTime = frozenState;
    }
}
