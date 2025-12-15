using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Activate Supression Indicator", story: "Activate Suppression Indicator [Bool]", category: "Action", id: "3b4c217d3e4b6d5375011b54634d176f")]
public partial class ActivateSupressionIndicatorAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Indicator;
    [SerializeReference] public BlackboardVariable<bool> Bool;

    protected override Status OnStart()
    {
        Indicator.Value.SetActive(Bool.Value);

        return Status.Success;
    }
}

