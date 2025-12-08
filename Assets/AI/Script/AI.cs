using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    enum States { Idle, Walking, Shooting}

    [SerializeField] States states = States.Idle;

    NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        
    }
}
