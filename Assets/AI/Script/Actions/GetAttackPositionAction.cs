using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Collections.Generic;
using Unity.AppUI.Core;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GetAttackPosition", story: "[Agent] gets attack [Target]", category: "Action", id: "d1135a497a6fe437a9ab53af8bdebed2")]
public partial class GetAttackPositionAction : Action
{
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Agent;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<Vector3> dir;
    [SerializeReference] public BlackboardVariable<Vector3> LastPos;

    protected override Status OnStart()
    {
        List<Transform> positions = CoverPositions(Agent.Value.transform.position, 100f);

        List<Transform> positionsInCover = new List<Transform>();

        if (positions.Count > 0)
        {
            foreach (Transform transform in positions)
            {
                if (PositionInCover(transform.position, dir))
                {
                    //Debug.Log(transform.gameObject.name);
                    positionsInCover.Add(transform);
                }
            }

            Vector3 pos = new Vector3(0, 0, 0);

            if (positionsInCover.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, positionsInCover.Count);
                pos = positionsInCover[index].position;
            }
            else
            {
                //FallBackPos();
                return Status.Success;
            }

            NavMeshHit navHit;
            if (NavMesh.SamplePosition(pos, out navHit, 100f, NavMesh.AllAreas))
            {
                Target.Value.position = navHit.position;
            }

            return Status.Success;
        }
        else
        {
            //FallBackPos();
        }

        return Status.Success;
    }

    private List<Transform> CoverPositions(Vector3 target, float seperation)
    {
        LayerMask CoverLayer = new LayerMask();
        CoverLayer += LayerMask.GetMask("CoverPositions");

        List<Transform> transforms = new List<Transform>();

        Collider[] hitColliders = Physics.OverlapSphere(target, seperation, CoverLayer);

        foreach (var hitCollider in hitColliders)
        {
            transforms.Add(hitCollider.transform);
        }

        return transforms;
    }

    private bool PositionInCover(Vector3 pos, Vector3 dir)
    {
        LayerMask CoverObjectsLayer = new LayerMask();
        CoverObjectsLayer += LayerMask.GetMask("Default");
        CoverObjectsLayer += LayerMask.GetMask("Terrain");

        //Debug.DrawRay(pos, dir, Color.yellow, 1f);

        RaycastHit hit;
        if (Physics.Raycast(pos, dir, out hit, 3f, CoverObjectsLayer))
        {
            //Debug.Log("hit");
            return true;
        }
        else
        {
            //Debug.Log("noHit");
            return false;
        }
    }
}

