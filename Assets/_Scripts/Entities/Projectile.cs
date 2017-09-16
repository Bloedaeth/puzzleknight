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
        Entity target = collidingObject.gameObject.GetComponent<Entity>();

        if(target)
        {
            Shield shield = null;
            if(target.transform.CompareTag("Player"))
                shield = target.GetComponent<Player>().Shield;
            else if(target.transform.CompareTag("Enemy"))
                shield = target.GetComponent<ShieldedEnemy>().Shield;

            if(shield != null && shield.IsBlocking)
            {
                Vector3 targetDir = target.transform.position - Self.transform.position;
                float angle = Vector3.Angle(-(target.transform.forward), targetDir);

                if(shield.BlockSuccessful(angle))
                    shield.Self.Stagger();
                else
                    Self.Attack(target, projectileDamage);
            }
            else if(!target.GetComponent<DeathAnimation>())
                    Self.Attack(target, projectileDamage);

        }

        gameObject.SetActive(false);
    }
}
