using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class SquadMovement : MonoBehaviour
{
    Squad squad;

    Vector3 targetPos;

    [SerializeField] LayerMask terrainLayer;
    [SerializeField] float speed = 5f;

    private void Awake()
    {
        squad = GetComponent<Squad>();
    }

    void FixedUpdate()
    {
        if(squad.charactersInSquad == null) { return; }

        List<Vector3> CharacterPos = new List<Vector3>();

        foreach(var AI in squad.charactersInSquad)
        {
            CharacterPos.Add(AI.transform.position);
        }

        Vector3 median = GetMeanVector(CharacterPos);

        RaycastHit hit;

        if (Physics.Raycast(this.transform.position, Vector3.down, out hit, Mathf.Infinity, terrainLayer))
        {
            median.y = hit.point.y;
        }

        targetPos = median;
    }

    private void Update()
    {
        float dist = Vector3.Distance(targetPos, this.transform.position);

        if(dist > 0.5f)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
    }

    private Vector3 GetMeanVector(List<Vector3> positions)
    {
        Vector3 meanVector = Vector3.zero;
        foreach (Vector3 pos in positions)
        {
            meanVector += pos;
        }
        return (positions.Count == 0 ? meanVector : meanVector / positions.Count);
    }
}
