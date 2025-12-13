using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;
using System.Diagnostics;
using Unity.VisualScripting;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Shooting", story: "Shoot", category: "Action", id: "3df717f97f3b3210ba9d74f69b2fbbae")]
public partial class ShootingAction : Action
{
    [Tooltip("The agent")]
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Agent;

    [Tooltip("Spread")]
    [SerializeReference] public BlackboardVariable<Vector3> Spread;

    [Tooltip("Damage")]
    [SerializeReference] public BlackboardVariable<Vector2> Damage;

    [Tooltip("Debug")]
    [SerializeReference] public BlackboardVariable<bool> debug;

    protected override Status OnStart()
    {
        LayerMask ShootableLayer = new LayerMask();

        ShootableLayer += LayerMask.GetMask("CoverPositions");

        if (Agent.Value.gameObject.tag == "Blue")
        {
            ShootableLayer += LayerMask.GetMask("Blue");
        }
        else
        {
            ShootableLayer += LayerMask.GetMask("Red");
        }

        RaycastHit hit;

        Vector3 RSpread = new Vector3(UnityEngine.Random.Range(-Spread.Value.x, Spread.Value.x),
                                      UnityEngine.Random.Range(-Spread.Value.y, Spread.Value.y),
                                      UnityEngine.Random.Range(-Spread.Value.z, Spread.Value.z));

        if (Physics.Raycast(Agent.Value.transform.position + new Vector3(0,1,0), Agent.Value.transform.forward + RSpread, out hit, Mathf.Infinity, ~ShootableLayer))
        {
            UnityEngine.Debug.Log("Hit" + hit.transform.gameObject.name + " " + hit.transform.gameObject.tag);

            if(hit.transform.gameObject.tag == "Blue" || hit.transform.gameObject.tag == "Red")
            {
                hit.transform.gameObject.GetComponent<I_Health>().Hit(UnityEngine.Random.Range(Damage.Value.x, Damage.Value.y));
            }

            if (debug)
            {
                Agent.Value.transform.GetChild(1).GetComponent<DebugScript>().SetBulletInfo(Agent.Value.transform.position, hit.point);
            }
        }

        return Status.Success;
    }
}

