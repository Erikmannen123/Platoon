using System.Collections.Generic;
using Unity.AppUI.Core;
using Unity.AppUI.UI;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour, I_PassTarget
{
    enum States { Idle, Walking, Shooting}

    [SerializeField] States states = States.Idle;

    NavMeshAgent agent;
    BehaviorGraphAgent behaviour;

    //public enum Directions { Null, North, East, South, West }

    //Directions directions = Directions.Null;

    Vector3 direction = new Vector3(0, 0, 0);

    public Vector3 squadDestination;
    [SerializeField] Transform targetPrefab;
    public Transform target;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        behaviour = GetComponent<BehaviorGraphAgent>();
        behaviour.SetVariableValue("Agent", agent);

        target = Instantiate(targetPrefab, this.transform.position, Quaternion.identity);
    }

    void Update()
    {
        if(agent != null)
        {
            if (target != null)
            {
                behaviour.SetVariableValue("Target", target);
            }
        }
    }

    public void PassTarget(Vector3 target)
    {
        MoveToLocation(target);
    }

    //Get random pos with in radius
    public void MoveToRandomLocation(Vector3 destination, float seperation)
    {
        Vector3 pos = destination + Random.insideUnitSphere * seperation;
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(pos, out navHit, 100f, NavMesh.AllAreas))
        {
            target.position = navHit.position;
        }
    }

    public void MoveToLocation(Vector3 destination)
    {
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(destination, out navHit, 100f, NavMesh.AllAreas))
        {
            target.position = navHit.position;
        }
    }
}
