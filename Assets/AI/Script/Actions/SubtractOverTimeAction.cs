using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SubtractOverTime", story: "Subtract [Value] with [ValueOverTime] [BelowZero]", category: "Action", id: "0c4a694dfbbaed68686ab4071ba9fdf8")]
public partial class SubtractOverTimeAction : Action
{
    [SerializeReference] public BlackboardVariable<float> Value;
    [SerializeReference] public BlackboardVariable<float> ValueOverTime;
    [SerializeReference] public BlackboardVariable<bool> BelowZero;

    protected override Status OnUpdate()
    {
        if(Value.Value <= 0)
        {
            if(BelowZero.Value == true)
            {
                Subtract();
            }
        }
        else
        {
            Subtract();
        }

        return Status.Success;
    }

    void Subtract()
    {
        Value.Value -= (ValueOverTime.Value * Time.deltaTime);
    }
}

