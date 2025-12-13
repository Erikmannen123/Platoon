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
    [Tooltip("The GameObject to show the text over.")]
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Agent;

    [Tooltip("The GameObject to show the text over.")]
    [SerializeReference] public BlackboardVariable<Transform> Enemy;

    [Tooltip("The GameObject to show the text over.")]
    [SerializeReference] public BlackboardVariable<Vector3> LastKnownPos;

    [Tooltip("The GameObject to show the text over.")]
    [SerializeReference] public BlackboardVariable<float> Angel;

    protected override Status OnUpdate()
    {
        Vector3 target;

        if (Enemy.Value != null)
        {
            target = Enemy.Value.position;
        }
        else
        {
            target = LastKnownPos.Value;
        }

        Vector3 direction = (target - Agent.Value.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        Agent.Value.transform.rotation = Quaternion.Slerp(Agent.Value.transform.rotation, lookRotation, Time.deltaTime * 5);

        Angel.Value = Vector3.Angle(Agent.Value.transform.forward, target - Agent.Value.transform.position);

        return Status.Success;

    }
}

