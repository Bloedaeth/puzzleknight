using GameLogging;
using UnityEngine;

public class Turret : Enemy
{
    public Transform turretMuzzle;

    public float range = 100.0f;

    [SerializeField] private AudioClip[] fireSounds;

    private Transform myTransform;
    private Transform player;

    private float fireTime;
    
    private void Start()
    {
        myTransform = transform;
        player = FindObjectOfType<Player>().GetComponent<Transform>();
    }

    private void Update()
    {
        if(Time.time > fireTime && Vector3.Distance(myTransform.position, player.position) < range)
        {
            BuildDebug.Log("Cannon firing: " + name, true);

            Projectile projectile = ObjectPooler.main.GetPooledObject().GetComponent<Projectile>();
            projectile.transform.position = turretMuzzle.position;
            projectile.transform.rotation = turretMuzzle.rotation;
            projectile.Self = this;
            projectile.forward = turretMuzzle.forward;
            projectile.gameObject.SetActive(true);
            fireTime = Time.time + Random.Range(1, 7);

            audio.clip = fireSounds[Random.Range(0, fireSounds.Length)];
            audio.Play();
        }
    }

    /// <summary>Checks if the entity can be attacked, and attacks them if so.</summary>
    /// <param name="target">The entity to attack.</param>
    /// <param name="damage">The damage to deal to the entity.</param>
    public override void Attack(Entity target, int damage)
    {
        base.Attack(target, damage);
    }
}

//public class Turret : MonoBehaviour {

//	private Transform myTransform;

//	//Enemy Player reference
//	private GameObject enemyPlayer;

//	//Turret Variables
//	public float range = 100.0f;
//	public float fireRate = 20.0f;
//	private float fireTime;

//	//Projectile
//	public GameObject turretProjectile;

//	//Turret Parts
//	public GameObject turretMuzzle;
//	public GameObject turretRaycast;

//	//Rotation Variables
//	public float rotationSpeed = 2.0f;
//	private float adjRotSpeed;
//	private Quaternion targetRotation;

//	// Use this for initialization
//	void Start () {
//		myTransform = this.transform;

//		enemyPlayer = GameObject.FindGameObjectWithTag ("Player");
//	}

//	// Update is called once per frame
//	void Update () {

//		//Raycast Detection
//		RaycastHit hit;
//		if (Physics.Raycast (turretRaycast.transform.position, -(turretRaycast.transform.position - enemyPlayer.transform.position).normalized, out hit, range)) {

//			//If hit has "Player" tag...
//			if (hit.transform.tag == "Player"){

//				//Fire Projectile
//				if (Time.time > fireTime) {
//					Instantiate (turretProjectile, turretMuzzle.transform.position, turretMuzzle.transform.rotation);
//					fireTime = Time.time + fireRate;
//				}

//				//Draw red debug line
//				Debug.DrawLine (turretRaycast.transform.position, hit.point, Color.red);
//			} else {
//				//Draw green debug line
//				Debug.DrawLine (turretRaycast.transform.position, hit.point, Color.green);
//			}
//		}


//	}
//}
