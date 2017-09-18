using UnityEngine;

public class Pendulum : MonoBehaviour, IFreezable
{
    public bool SlowedTime { get; set; }

    private void Awake()
    {
		SlowedTime = false;
	}
}
