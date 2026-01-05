using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CompareTag", story: "Is [Gameobject] Tag equal to [Tag]", category: "Conditions", id: "dbd8c47c46a2f87d6dcdfdc9e743d6a9")]
public partial class CompareTagCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Gameobject;
    [SerializeReference] public BlackboardVariable<string> Tag;

    public override bool IsTrue()
    {
        if(Gameobject.Value.tag == Tag)
        {
            return true;
        }

        return false;
    }
}
