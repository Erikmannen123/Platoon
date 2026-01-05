using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Timer", story: "Timer [Value]", category: "Action", id: "f1b920e8df9936fcad311c4e35cecb75")]
public partial class TimerAction : Action
{
    [SerializeReference] public BlackboardVariable<float> Value;

    protected override Status OnStart()
    {
        Value.Value -= UnityEngine.Time.deltaTime;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Value.Value -= UnityEngine.Time.deltaTime;

        if (Value.Value <= 0)
        {
            Value.Value = 0;
            return Status.Success;
        }
        else
        {
            return Status.Running;
        }
    }
}

