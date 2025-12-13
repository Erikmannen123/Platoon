using System.Diagnostics;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
    Transform start;
    Transform end;

    public void SetInfo(Transform start, Transform end)
    {
        this.start = start;
        this.end = end;
    }

    void OnDrawGizmos()
    {
        if (start != null && end != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(start.position + new Vector3(0,1,0), end.position + new Vector3(0, 1, 0));
        }
    }
}
