using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RotateTowards", story: "Rotate Agent", category: "Action", id: "c40280397f68eda0fdda21b0a0687ae4")]
public partial class RotateTowardsAction : Action
{
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Agent;

    [SerializeReference] public BlackboardVariable<Transform> Enemy;

    [SerializeReference] public BlackboardVariable<Vector3> LastKnownPos;

    [SerializeReference] public BlackboardVariable<Vector3> TakingFireFrom;

    [SerializeReference] public BlackboardVariable<float> Angel;

    protected override Status OnUpdate()
    {
        Vector3 target = new Vector3(0, 0, 0);

        Vector3 direction;

        if (Enemy.Value != null)
        {
            target = Enemy.Value.position;
            direction = (target - Agent.Value.transform.position).normalized;
        }
        else if(LastKnownPos.Value != Vector3.zero)
        {
            target = LastKnownPos.Value;
            direction = (target - Agent.Value.transform.position).normalized;
        }
        else if (TakingFireFrom.Value != Vector3.zero)
        {
            direction = TakingFireFrom;
        }
        else
        {
            return Status.Failure;
        }

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        Agent.Value.transform.rotation = Quaternion.Slerp(Agent.Value.transform.rotation, lookRotation, Time.deltaTime * 5);

        Angel.Value = Vector3.Angle(Agent.Value.transform.forward, target - Agent.Value.transform.position);

        return Status.Success;

    }
}

