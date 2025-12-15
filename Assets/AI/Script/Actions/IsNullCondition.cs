using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsNull", story: "Check if [value] is null", category: "Variable Conditions", id: "aeed56b4bf6dad342a87555e44bdff2d")]
public partial class IsNullCondition : Condition
{
    [SerializeReference] public BlackboardVariable Value;

    public override bool IsTrue()
    {
        if (Value.ObjectValue == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
