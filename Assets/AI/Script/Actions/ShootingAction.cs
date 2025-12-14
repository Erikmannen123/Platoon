using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;
using System.Diagnostics;
using Unity.VisualScripting;
using static UnityEngine.GraphicsBuffer;

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

    [Tooltip("Suppress value")]
    [SerializeReference] public BlackboardVariable<float> SuppresValue;

    [Tooltip("Suppress radius")]
    [SerializeReference] public BlackboardVariable<float> SuppresRadius;

    [Tooltip("Debug")]
    [SerializeReference] public BlackboardVariable<bool> debug;

    protected override Status OnStart()
    {
        LayerMask ShootableLayer = new LayerMask();
        LayerMask SuppressLayer = new LayerMask();

        ShootableLayer += LayerMask.GetMask("CoverPositions");

        if (Agent.Value.gameObject.tag == "Blue")
        {
            ShootableLayer += LayerMask.GetMask("Blue");
            SuppressLayer += LayerMask.GetMask("Red");
        }
        else
        {
            ShootableLayer += LayerMask.GetMask("Red");
            SuppressLayer += LayerMask.GetMask("Blue");
        }

        RaycastHit hit;

        Vector3 RSpread = new Vector3(UnityEngine.Random.Range(-Spread.Value.x, Spread.Value.x),
                                      UnityEngine.Random.Range(-Spread.Value.y, Spread.Value.y),
                                      UnityEngine.Random.Range(-Spread.Value.z, Spread.Value.z));

        if (Physics.Raycast(Agent.Value.transform.position + new Vector3(0,1,0), Agent.Value.transform.forward + RSpread, out hit, Mathf.Infinity, ~ShootableLayer))
        {
            //UnityEngine.Debug.Log("Hit" + hit.transform.gameObject.name + " " + hit.transform.gameObject.tag);

            if(hit.transform.gameObject.tag == "Blue" || hit.transform.gameObject.tag == "Red")
            {
                hit.transform.gameObject.GetComponent<I_Health>().Hit(UnityEngine.Random.Range(Damage.Value.x, Damage.Value.y));
            }

            float dist = Vector3.Distance(hit.transform.position, Agent.Value.transform.position);

            RaycastHit[] SuppresssionHit = Physics.SphereCastAll(Agent.Value.transform.position + new Vector3(0, 1, 0), SuppresRadius, Agent.Value.transform.forward + RSpread, dist, SuppressLayer);

            foreach(var collider in SuppresssionHit)
            {
                BlackboardReference BR = collider.transform.gameObject.GetComponent<BehaviorGraphAgent>().BlackboardReference;

                BlackboardVariable<float> currentSupresion;
                BR.GetVariable("Suppressed", out currentSupresion);
                BR.SetVariableValue("Suppressed", currentSupresion + SuppresValue);

                Vector3 direction = (Agent.Value.transform.position - collider.transform.position).normalized;
                direction.y = 0;
                BR.SetVariableValue("TakingFireFrom", direction);
            }


            if (debug)
            {
                Agent.Value.transform.GetChild(1).GetComponent<DebugScript>().SetBulletInfo(Agent.Value.transform.position, hit.point);
            }
        }

        return Status.Success;
    }
}

