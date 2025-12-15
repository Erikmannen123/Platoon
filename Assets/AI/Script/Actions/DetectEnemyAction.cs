using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.HDROutputUtils;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Unity.VisualScripting;
using NUnit;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "DetectEnemy", story: "Agent looks for target", category: "Action", id: "7e2a6b80a9c2aeab07db8d7c6ec0916b")]
public partial class DetectEnemyAction : Action
{
    [Tooltip("Actor that detects")]
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Agent;

    [Tooltip("If enemy is found it will return here")]
    [SerializeReference] public BlackboardVariable<Transform> Enemy;

    [Tooltip("Distance of detection")]
    [SerializeReference] public BlackboardVariable<float> DetectionDistance;

    [Tooltip("last seen pos")]
    [SerializeReference] public BlackboardVariable<Vector3> LastKnownPos;

    [Tooltip("Debug")]
    [SerializeReference] public BlackboardVariable<bool> debug;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        LayerMask EnemyLayer = new LayerMask();

        if (Agent.Value.tag == "Blue")
        {
            EnemyLayer = LayerMask.GetMask("Red");
        }
        else
        {
            EnemyLayer = LayerMask.GetMask("Blue");
        }
        
        Collider[] hitColliders = Physics.OverlapSphere(Agent.Value.transform.position, DetectionDistance, EnemyLayer);

        if(hitColliders.Length > 0)
        {
            List<Transform> hitTransforms = new List<Transform>();

            LayerMask LineOfSightLayer = new LayerMask();

            LineOfSightLayer += LayerMask.GetMask("Red");
            LineOfSightLayer += LayerMask.GetMask("Blue");
            LineOfSightLayer += LayerMask.GetMask("CoverPositions");

            foreach(var hit in hitColliders)
            {
                if (!Physics.Linecast(Agent.Value.transform.position, hit.transform.position, ~LineOfSightLayer))
                {
                    hitTransforms.Add(hit.transform);
                }
            }
            
            if(hitTransforms.Count > 0)
            {
                //Get closest
                int closestIndex = 0;
                float closestDist = 0f;

                for (int i = 0; i < hitTransforms.Count; i++)
                {
                    float dist;
                    dist = Vector3.Distance(Agent.Value.transform.position, hitTransforms[i].position);

                    if (dist <= closestDist || closestDist == 0f)
                    {
                        closestIndex = i;
                    }
                }

                Enemy.Value = hitTransforms[closestIndex];

                LastKnownPos.Value = Enemy.Value.position;

                if (debug)
                {
                    Agent.Value.transform.GetChild(1).GetComponent<DebugScript>().SetLineOfSightInfo(Agent.Value.transform, Enemy.Value);
                }

                return Status.Success;
            }
        }

        if (debug)
        {
            Agent.Value.transform.GetChild(1).GetComponent<DebugScript>().SetLineOfSightInfo(null, null);
        }


        Enemy.Value = null;
        return Status.Success;
    }
}

