using System;
using System.Collections.Generic;
using TMPro;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.HDROutputUtils;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GetCoverPosition", story: "GetCoverPosition", category: "Action", id: "6dc7b7784c9247c45959c651720c8a45")]
public partial class GetCoverPositionAction : Action
{
    [Tooltip("Agent")]
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Agent;

    [Tooltip("Agent")]
    [SerializeReference] public BlackboardVariable<Transform> Target;

    [Tooltip("Debug")]
    [SerializeReference] public BlackboardVariable<Vector3> LastKnownPos;

    [Tooltip("Debug")]
    [SerializeReference] public BlackboardVariable<Vector3> dir;

    protected override Status OnStart()
    {
        Transform agentTransform = Agent.Value.transform;

        List<Transform> positions = CoverPositions(Agent.Value.transform.position, 30f);

        List<Transform> positionsInCover = new List<Transform>();

        if (positions.Count > 0)
        {
            if (LastKnownPos.Value != Vector3.zero)
            {
                foreach (Transform transform in positions)
                {
                    if (PositionInCoverPosition(transform.position, LastKnownPos))
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
                    if (PositionInCoverDirection(transform.position, dir))
                    {
                        //Debug.Log(transform.gameObject.name);
                        positionsInCover.Add(transform);
                    }
                }
            }

            

            Vector3 closestPos = new Vector3(0,0,0);

            if(positionsInCover.Count > 0)
            {
                float closestDist = 0;

                foreach(Transform transform in positionsInCover)
                {
                    float dist = Vector3.Distance(Agent.Value.transform.position, transform.position);

                    if(dist <= closestDist || closestDist == 0f)
                    {
                        closestDist = dist;
                        closestPos = transform.position;
                    }
                }

                /*
                int index = UnityEngine.Random.Range(0, positionsInCover.Count);
                pos = positionsInCover[index].position;
                */
            }
            else
            {
                FallBackPos();
                return Status.Success;
            }

            NavMeshHit navHit;
            if (NavMesh.SamplePosition(closestPos, out navHit, 100f, NavMesh.AllAreas))
            {
                Target.Value.position = navHit.position;
            }

            return Status.Success;
        }
        else
        {
            FallBackPos();
        }

        return Status.Success;
    }

    void FallBackPos()
    {
        //Vector3 pos = (-Agent.Value.transform.forward) + new Vector3(0.5f, 0f, 0.5f) * 20f;

        Vector3 pos = UnityEngine.Random.insideUnitSphere + ((Agent.Value.transform.forward) * -3f) * 20f;

        NavMeshHit navHit;
        if (NavMesh.SamplePosition(pos, out navHit, 100f, NavMesh.AllAreas))
        {
            Target.Value.position = navHit.position;

            //Debug.Log("FallBackPos" + navHit.position);
        }
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

