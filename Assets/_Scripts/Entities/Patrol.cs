using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

//
// ~~ REDUNDANT CLASS ~~
//
// USED FOR SEAN'S INITIAL TESTING ONLY
//

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ThirdPersonCharacter))]
public class Patrol : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private ThirdPersonCharacter character;
    private Transform[] patrolPoints;
    private int curPoint = 0;

    private void Start()
    {
        patrolPoints = GameObject.Find("Patrol Points").GetComponentsInChildren<Transform>();

        navAgent = GetComponent<NavMeshAgent>();
        character = GetComponent<ThirdPersonCharacter>();

        GetNextPatrolPoint();
    }

    private void Update()
    {
        if(navAgent.remainingDistance < navAgent.stoppingDistance)
        {
			character.Move(Vector3.zero, false, false, false);
            Invoke("GetNextPatrolPoint", 2f);
        }
    }

    private void GetNextPatrolPoint()
    {
        if(patrolPoints.Length == 0)
            Debug.LogError("No patrol points set");

        curPoint = (curPoint + 1) % patrolPoints.Length;
        navAgent.SetDestination(patrolPoints[curPoint].position);
		character.Move(navAgent.desiredVelocity, false, false, false);
    }
}
