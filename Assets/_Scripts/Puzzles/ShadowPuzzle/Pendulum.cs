using UnityEngine;

public class Pendulum : MonoBehaviour, IFreezable
{
    public bool SlowedTime { get; set; }

	//bool SlowedTimePrev;
	//public bool SlowedTimeDown;
	//public bool SlowedTimeUp;

    private void Awake()
    {
		SlowedTime = false;
	}

	/*void Update() {
		
		SlowedTimeDown = SlowedTime && !SlowedTimePrev;
		SlowedTimeUp = !SlowedTime && SlowedTimePrev;




		SlowedTimePrev = SlowedTime;
	}*/
}
