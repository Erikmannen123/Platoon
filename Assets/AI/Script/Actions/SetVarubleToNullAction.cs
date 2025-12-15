using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set Varuble to null", story: "Set [Varuble] to null", category: "Action", id: "b9fe0f75fd322f137ecbfc9a0a18d543")]
public partial class SetVarubleToNullAction : Action
{
    [SerializeReference] public BlackboardVariable Varuble;

    protected override Status OnStart()
    {
        Varuble.ObjectValue = null;
        return Status.Success;
    }
}

