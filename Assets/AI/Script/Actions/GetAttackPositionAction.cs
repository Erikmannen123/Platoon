using System;
using System.Collections.Generic;
using TMPro;
using Unity.AppUI.Core;
using Unity.Behavior;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.Image;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GetAttackPosition", story: "[Agent] gets attack [Target]", category: "Action", id: "d1135a497a6fe437a9ab53af8bdebed2")]
public partial class GetAttackPositionAction : Action
{
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Agent;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<Vector3> TakingFireFrom;
    [SerializeReference] public BlackboardVariable<Vector3> LastPos;

    protected override Status OnStart()
    {
        Vector3 advancePosition = Vector3.zero;
        Vector3 direction = Vector3.zero;

        if (LastPos.Value != Vector3.zero)
        {
            direction = (LastPos.Value - Agent.Value.transform.position).normalized;
        }
        else if (TakingFireFrom.Value != Vector3.zero)
        {
            direction = TakingFireFrom.Value.normalized;
        }
        else
        {
            //Abort get to the squad maker position
            Debug.Log("no Attack position");
            return Status.Success;
        }

        advancePosition = Agent.Value.transform.position + direction * 20f;

        List<Transform> positions = CoverPositions(advancePosition, 30f);

        List<Transform> positionsInCover = new List<Transform>();

        if (positions.Count > 0)
        {
            if (LastPos.Value != Vector3.zero)
            {
                foreach (Transform transform in positions)
                {
                    if (PositionInCoverPosition(transform.position, LastPos))
                    {
                        //Debug.Log(transform.gameObject.name);
                        positionsInCover.Add(transform);
                    }
                }
            }
            else
            {
                foreach (Transform transform in positions)
                {
                    if (PositionInCoverDirection(transform.position, TakingFireFrom))
                    {
                        //Debug.Log(transform.gameObject.name);
                        positionsInCover.Add(transform);
                    }
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
                //Abort get to the squad maker position
                Debug.Log("no Attack position");
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
            //Abort get to the squad maker position
            Debug.Log("no Attack position");
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

    private bool PositionInCoverPosition(Vector3 currentPos, Vector3 lastPos)
    {
        LayerMask CoverObjectsLayer = new LayerMask();
        CoverObjectsLayer += LayerMask.GetMask("Default");
        CoverObjectsLayer += LayerMask.GetMask("Terrain");

        //Debug.DrawRay(pos, dir, Color.yellow, 1f);

        Vector3 dir = (lastPos - currentPos).normalized;

        RaycastHit hit;
        if (Physics.Raycast(currentPos, dir, out hit, 3f, CoverObjectsLayer))
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

    private bool PositionInCoverDirection(Vector3 pos, Vector3 dir)
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

