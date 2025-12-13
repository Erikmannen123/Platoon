using System.Collections.Generic;
using Unity.AppUI.Core;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static AI;
using static UnityEditor.PlayerSettings;

public class Squad : MonoBehaviour, I_PassTarget
{
    [SerializeField] private Vector3 target;

    [SerializeField] private GameObject character;

    [SerializeField] public List<AI> charactersInSquad = new List<AI>();

    [SerializeField] private float seperation = 5f;

    [SerializeField] int SquadCount = 4;

    [SerializeField] private LayerMask terrainLayer;

    [SerializeField] private LayerMask CoverLayer;

    [SerializeField] private LayerMask CoverObjectsLayer;

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
            newCharacter.GetComponent<TeamDetection>().SetTeam(this.gameObject.tag);
            charactersInSquad.Add(newCharacter.GetComponent<AI>());
        }
    }

    void Move(Vector3 target, float seperation, List<AI> charactersInSquad)
    {
        //For beter preformance enable this so it calculates the squad cover direction instead of the soldier cover direction
        //Queue<Transform> CoverPos = CoverPositions(target, seperation, DirectionToTakeCoverFrom);

        List<Transform> positions = CoverPositions(target, seperation);

        foreach (AI ai in charactersInSquad)
        {
            bool foundCover = false;

            foreach(Transform pos in positions)
            {
                Vector3 dir = ai.direction;

                if (dir == new Vector3(0, 0, 0))
                {
                    dir = DirectionToTakeCoverFrom;
                }

                if(dir == new Vector3(0, 0, 0))
                {
                    ai.MoveToLocation(pos.position);
                    positions.Remove(pos);
                    foundCover = true;
                    break;
                }

                if (PositionInCover(pos.position, dir))
                {
                    ai.MoveToLocation(pos.position);
                    //Debug.Log(pos.gameObject.name);
                    positions.Remove(pos);

                    foundCover = true;
                    break;
                }
            }

            if (!foundCover)
            {
                ai.MoveToRandomLocation(target, seperation);
            }
        }
    }

    private List<Transform> CoverPositions(Vector3 target, float seperation)
    {
        List<Transform> transforms = new List<Transform>();

        Collider[] hitColliders = Physics.OverlapSphere(target, seperation, CoverLayer);

        foreach (var hitCollider in hitColliders)
        {
            transforms.Add(hitCollider.transform);
        }

        return transforms;
    }

    private bool PositionInCover(Vector3 pos, Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(pos, dir, out hit, 3f, CoverObjectsLayer))
        {
            //Debug.Log("hit");
            return true;
        }
        else 
        {
            //Debug.Log("noHit");
            return false; 
        }
    }
}
