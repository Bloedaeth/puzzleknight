using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour {

    private Rigidbody rbody;
    public float fallDelay;

    private void Start()
    {
        rbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        ///longer delay (2 seconds?) and a "shake" animation before falling would be good at some point
        yield return new WaitForSeconds(fallDelay);
        rbody.isKinematic = false;
        GetComponent<Collider>().isTrigger = true;
        yield return 0;
    }
}
