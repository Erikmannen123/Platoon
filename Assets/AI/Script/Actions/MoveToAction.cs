using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveTo", story: "Move to transform.", category: "Action", id: "df11c6a6dbb51ab7f66a09c050909822")]
public partial class MoveToAction : Action
{
    [Tooltip("The GameObject to show the text over.")]
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Agent;

    [Tooltip("The GameObject to show the text over.")]
    [SerializeReference] public BlackboardVariable<Transform> Target;

    [Tooltip("The time in seconds for the text to show.")]
    [SerializeReference] public BlackboardVariable<float> Speed = new BlackboardVariable<float>(2.0f);

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(Agent.Value == null || Target.Value == null) { return Status.Running; }

        float dist = Vector3.Distance(Target.Value.position, Agent.Value.transform.position);

        if(dist <= 0.2f)
        {
            return Status.Success;
        }
        else
        {
            Agent.Value.speed = Speed;
            Agent.Value.SetDestination(Target.Value.position);
            return Status.Running;
        }
    }

    protected override void OnEnd()
    {
    }
}

