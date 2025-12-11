using System.Collections.Generic;
using Unity.AppUI.Core;
using Unity.AppUI.UI;
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

    [SerializeField] private LayerMask CoverLayer;

    [SerializeField] private Vector3 DirectionToTakeCoverFrom;

    void I_PassTarget.PassTarget(Vector3 target)
    {
        this.target = target;
        Move(target, seperation, charactersInSquad);
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

    void Move(Vector3 target, float seperation, List<AI> charactersInSquad)
    {
        Queue<Transform> CoverPos = CoverPositions(target, seperation, DirectionToTakeCoverFrom);

        foreach (AI ai in charactersInSquad)
        {
            if(CoverPos.Count > 0)
            {
                ai.MoveToLocation(CoverPos.Peek().position);
                CoverPos.Dequeue();

                Debug.Log("iakuwjhdia");
            }
            else
            {
                ai.MoveToRandomLocation(target, seperation);
            }
        }
    }

    private Queue<Transform> CoverPositions(Vector3 target, float seperation, Vector3 dir)
    {
        Queue<Transform> transforms = new Queue<Transform>();

        Collider[] hitColliders = Physics.OverlapSphere(target, seperation, CoverLayer);

        foreach (var hitCollider in hitColliders)
        {
            if (dir == new Vector3(0, 0, 0))
            {
                transforms.Enqueue(hitCollider.transform);
                break;
            }
            else
            {
                if(PositionInCover(hitCollider.transform.position, dir))
                {
                    transforms.Enqueue(hitCollider.transform);
                    break;
                }
            }
        }

        return transforms;
    }

    private bool PositionInCover(Vector3 pos, Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(pos, dir, out hit, 3f))
        {
            Debug.Log("hit");
            return true;
        }
        else 
        {
            Debug.Log("noHit");
            return false; 
        }
    }
}
