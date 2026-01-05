using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.AI;
using static UnityEngine.InputSystem.OnScreen.OnScreenStick;
using Unity.Behavior;

public class Spawn : MonoBehaviour
{
    [SerializeField] private Queue<GameObject> spawnList = new Queue<GameObject>();
    [SerializeField] private Queue<float> spawnListTimer = new Queue<float>();

    [SerializeField] private float time = 10f;

    private void Update()
    {
        if (spawnListTimer.Count > 0)
        {
            Queue<float> temp = new Queue<float>();

            for (int i = 0; i < spawnListTimer.Count; i++)
            {
                float t = spawnListTimer.Dequeue();
                t -= Time.deltaTime;
                temp.Enqueue(t);
            }

            spawnListTimer.Clear();
            spawnListTimer = temp;

            if(spawnListTimer.Peek() <= 0f)
            {
                SpawnSoldier();
            }
        }
    }

    public void AddToQueue(GameObject soldier)
    {
        spawnList.Enqueue(soldier);
        spawnListTimer.Enqueue(time);
    }

    private void SpawnSoldier()
    {
        GameObject go = spawnList.Dequeue();
        spawnListTimer.Dequeue();

        NavMeshHit navHit;
        if (NavMesh.SamplePosition(this.transform.position, out navHit, 10f, NavMesh.AllAreas))
        {
            go.transform.position = navHit.position;

            BehaviorGraphAgent behaviour = go.GetComponent<BehaviorGraphAgent>();
            behaviour.SetVariableValue("State", State.Walking);

            go.GetComponent<Health>().health = 100f;

            AI ai = go.GetComponent<AI>();
            ai.target.position = ai.Squad.position;

            go.SetActive(true);
        }
    }
}
