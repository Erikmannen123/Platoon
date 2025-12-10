using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Squad : MonoBehaviour, I_PassTarget
{
    [SerializeField] private Vector3 target;

    [SerializeField] private GameObject character;

    [SerializeField] public List<AI> charactersInSquad = new List<AI>();

    [SerializeField] private float seperation = 5f;

    [SerializeField] private LayerMask terrainLayer;

    void I_PassTarget.PassTarget(Vector3 target)
    {
        this.target = target;
    }

    private void Awake()
    {
        for(int i = 0; i < 10; i++)
        {
            SpawnSquad(character, charactersInSquad);
        }
    }

    void SpawnSquad(GameObject character, List<AI> charactersInSquad)
    {
        Vector3 pos = this.transform.position + new Vector3(Random.Range(-seperation, seperation), 100, Random.Range(-seperation, seperation));

        RaycastHit hit;

        if (Physics.Raycast(pos, Vector3.down, out hit, Mathf.Infinity, terrainLayer))
        {
            NavMeshHit navHit;
            if (NavMesh.SamplePosition(pos, out navHit, 100f, NavMesh.AllAreas))
            {
                GameObject newCharacter = Instantiate(character, navHit.position, Quaternion.identity);

                charactersInSquad.Add(newCharacter.GetComponent<AI>());
            }
        }
    }
}
