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

    public enum Directions { Null, North, East, South, West }

    Directions directions = Directions.Null;

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
        SetNewTarget(target, 0, Directions.Null);
    }

    public void SetNewTarget(Vector3 destination, float seperation, Directions squadDir)
    {
        Directions dir = GetCoverDirection(directions);

        if(dir == Directions.Null)
        {
            dir = GetCoverDirection(squadDir);
        }

        if (dir == Directions.Null)
        {
            SpawnSquad(destination, seperation);
        }
    }

    //Get direction to tack cover from
    private Directions GetCoverDirection(Directions dir)
    {
        switch (dir)
        {
            case Directions.Null:
                return Directions.Null;

            case Directions.North:
                return Directions.North;

            case Directions.East:
                return Directions.East;

            case Directions.South:
                return Directions.South;

            case Directions.West:
                return Directions.West;
        }
        return Directions.Null;
    }

    //Get random pos with in radius
    private void SpawnSquad(Vector3 destination, float seperation)
    {
        Vector3 pos = destination + Random.insideUnitSphere * seperation;
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(pos, out navHit, 100f, NavMesh.AllAreas))
        {
            target.position = navHit.position;
        }
    }

}
