using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Enemy Self;

    public float projectileSpeed = 40.0f;
    public int projectileDamage = 5;
    public float despawnTime = LIFETIME;
    public Vector3 forward;
    
    private Vector3 origin;
    private Vector3 endPos;
    private float step;

    private const float LIFETIME = 5f;

    private void OnEnable()
    {
        despawnTime = Time.time + LIFETIME;
        origin = transform.position;
        endPos = origin + (forward * projectileSpeed);
        step = 0;
    }

    private void Update()
    {
        step += Time.deltaTime / LIFETIME;
        transform.position = Vector3.Lerp(origin, endPos, step);

        if(Time.time >= despawnTime)
            gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        Entity target = other.gameObject.GetComponent<Entity>();
        if(target == Self)
            return;

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
