using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static AI;

public class Squad : MonoBehaviour, I_PassTarget
{
    [SerializeField] private Vector3 target;

    [SerializeField] private GameObject character;

    [SerializeField] public List<AI> charactersInSquad = new List<AI>();

    [SerializeField] private float seperation = 5f;

    [SerializeField] int SquadCount = 4;

    [SerializeField] private LayerMask terrainLayer;

    [SerializeField] private Vector3 DirectionToTakeCoverFrom;

    void I_PassTarget.PassTarget(Vector3 target)
    {
        this.target = target;
        Move(target, charactersInSquad);
    }

    private void Awake()
    {
        for(int i = 0; i < SquadCount; i++)
        {
            SpawnSquad(character, charactersInSquad);
        }
    }

    void SpawnSquad(GameObject character, List<AI> charactersInSquad)
    {
        Vector3 pos = this.transform.position + Random.insideUnitSphere * seperation;
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(pos, out navHit, 100f, NavMesh.AllAreas))
        {
            GameObject newCharacter = Instantiate(character, navHit.position, Quaternion.identity);

            charactersInSquad.Add(newCharacter.GetComponent<AI>());
        }
    }

    void Move(Vector3 target, List<AI> charactersInSquad)
    {
        foreach(AI ai in charactersInSquad)
        {
            ai.SetNewTarget(target, seperation, AI.Directions.Null);
        }
    }
}
