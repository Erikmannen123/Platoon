using System.Diagnostics;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
    Transform start;
    Transform end;

    Vector3 bulletStart;
    Vector3 bulletHit;

    public void SetLineOfSightInfo(Transform start, Transform end)
    {
        this.start = start;
        this.end = end;
    }

    public void SetBulletInfo(Vector3 start, Vector3 hit)
    {
        bulletStart = start;
        bulletHit = hit;
    }

    void OnDrawGizmos()
    {
        if (start != null && end != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(start.position + new Vector3(0, 1, 0), end.position + new Vector3(0, 1, 0));
        }

        if (bulletStart != new Vector3(0, 0, 0) && bulletHit != new Vector3(0,0,0))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(bulletStart + new Vector3(0, 1, 0), bulletHit);
        }
    }
}
