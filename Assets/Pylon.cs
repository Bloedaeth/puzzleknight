using UnityEngine;

public class Pylon : MonoBehaviour
{
    public int ID;
    private bool isActive = false;

    private float SPEED_MODIFIER = 5f;
    private float MIN_HEIGHT = 470f;
    private float MAX_HEIGHT = 490f;

    private void Update()
    {
		if(transform.localPosition.y < MAX_HEIGHT && isActive)
            transform.localPosition += transform.up * Time.deltaTime * SPEED_MODIFIER;
        else if(transform.localPosition.y > MIN_HEIGHT && !isActive)
            transform.localPosition -= transform.up * Time.deltaTime * SPEED_MODIFIER;
	}

    public void SetPylonActive(bool val)
    {
        isActive = val;
        FindObjectOfType<BossEnemy>().BossScaleMult += 1f;
    }
}
