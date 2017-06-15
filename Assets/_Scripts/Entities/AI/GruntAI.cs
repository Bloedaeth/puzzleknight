using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GruntAI : MonoBehaviour {
    public GameObject target;

    public float wanderRadius = 10;
    public float wanderTimer = 5;
    public float timer;

    public float attackSpeed = 5;
    public float timer2;

    public NavMeshAgent agent;

    public int newState = 1;
    private int currentState = 1;

    public float alertRadius = 5;
    public float combatRadius = 8;

    public bool isAlerted = false;
    public bool isInCombat = false;
    public bool isAttacking = false;

    private float fieldOfView = 80.0f;
    public float speed = -2;

    // Use this for initialization
    void Start()
    {
        try
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        catch
        {
            target = null;
        }
        timer = wanderTimer;
    }

    // Update is called once per frame
    void Update()
    {
        currentState = newState;
        switch (currentState)
        {
            //Wait
            case 0:
                Wait();
                break;
            //Wander
            case 1:
                Wander();
                break;
            //Combat
            case 2:
                Combat();
                break;
            //Pursuit
            case 3:
                Pursuit();
                break;

        }

        SwitchStates();
    }

    private void Wander()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, 1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    private void Wait()
    {
        GetComponent<Animator>().SetBool("Walking", false);
        GetComponent<Animator>().SetBool("Running", false);
    }

    private void Combat()
    {
        timer2 += Time.deltaTime;
        if (timer2 >= attackSpeed)
        {
            AttackPlayer();

            timer2 = 0;
        }

        AISpacing();

        if (Vector3.Distance(transform.position, target.transform.position) > combatRadius)
        {
            isInCombat = false;
        }

        AlertOthers();
    }

    private void Pursuit()
    {
        GetComponent<Animator>().SetBool("Running", true);

        AlertOthers();
        agent.destination = target.transform.position;
        transform.LookAt(target.transform.position);
    }

    private void SwitchStates()
    {

        if (PlayerDetection() == true)
        {
            isAlerted = true;
        }
        else
        {
            isAlerted = false;
        }

        if (isAlerted == true && isInCombat == false)
        {
            newState = 3;
        }
        else if (isAlerted == true && isInCombat == true)
        {
            newState = 2;
            agent.SetDestination(transform.position);
        }
        else
        {
            newState = 1;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    private void AlertOthers()
    {
        Vector3 aiPosition = transform.position;

        if (isAlerted == true)
        {
            Collider[] hitColliders = Physics.OverlapSphere(aiPosition, alertRadius);

            foreach (Collider hit in hitColliders)
            {
                if (hit.tag == "Enemy")
                {
                    RaycastHit raycastHit;
                    Vector3 rayDirection = hit.transform.position - aiPosition;
                    Debug.DrawRay(aiPosition, rayDirection);

                    if (Physics.Raycast(aiPosition, rayDirection, out raycastHit))
                    {

                        if (raycastHit.transform.tag == "Enemy")
                        {
                            if (hit.GetComponent<GruntAI>().isAlerted == false)
                            {
                                hit.GetComponent<GruntAI>().AlertMyself();
                            }

                        }
                    }
                }
            }
        }
    }

    private bool PlayerDetection()
    {
        RaycastHit hit;
        Vector3 rayDirection = target.transform.position - transform.position;

        float distanceToPlayer = Vector3.Distance(transform.position, target.transform.position);

        if (Physics.Raycast(transform.position, rayDirection, out hit))
        {

            if ((hit.transform.tag == target.tag) && (distanceToPlayer <= alertRadius))
            {
                return true;
            }
        }

        if ((Vector3.Angle(rayDirection, transform.forward)) <= fieldOfView)
        {
            if (Physics.Raycast(transform.position, rayDirection, out hit))
            {
                if (hit.transform.tag == target.tag)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
    }

    public void AlertMyself()
    {
        print("Pass 1");
        isAlerted = true;
    }

    public void CombatMyself()
    {
        isInCombat = true;
    }

    private void AttackPlayer()
    {
        //Attack Function
        //Attack Animation
        GetComponent<Animator>().SetTrigger("Attacking");

        isAttacking = true;
        //transform.position = Vector3.MoveTowards(transform.position,
            //target.transform.position, Vector3.Distance(transform.position, target.transform.position));
        print ("Attacked"); 
        isAttacking = false;
    }

    private void AISpacing()
    {
        Vector3 aiPosition = transform.position;

        Collider[] hitColliders = Physics.OverlapSphere(aiPosition, 2);

        foreach (Collider hit in hitColliders)
        {
            if (hit.transform.tag == "Enemy" && isAttacking == false)
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, hit.transform.position, step);
            }
        }
    }
}
