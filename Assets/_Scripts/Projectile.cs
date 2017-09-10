using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Enemy Self;

    public float projectileSpeed = 40.0f;
    public int projectileDamage = 5;
    public float despawnTime = 5.0f;
    public Vector3 forward;

    private Transform myTransform;

    private void Start()
    {
        myTransform = transform;
    }
    private void OnEnable()
    {
        despawnTime = Time.time + despawnTime;
    }

    private void Update()
    {
        myTransform.position = Vector3.Lerp(myTransform.position, myTransform.position + forward, Time.deltaTime * projectileSpeed);

        if(Time.time >= despawnTime)
            gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collidingObject)
    {
        Entity entity = collidingObject.gameObject.GetComponent<Entity>();
        if(entity)
            Self.Attack(entity, projectileDamage);

        gameObject.SetActive(false);
    }
}
