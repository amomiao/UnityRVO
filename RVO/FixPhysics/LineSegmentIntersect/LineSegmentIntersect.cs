using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSegmentIntersect : MonoBehaviour
{
    public Transform aStart;
    public Transform aEnd;
    public Transform bStart;
    public Transform bEnd;

    private void Awake()
    {
        GameObject obj;
        LineRenderer lr;
        obj = new GameObject();
        obj.transform.SetParent(this.gameObject.transform);
        lr = obj.AddComponent<LineRenderer>();
        lr.startWidth = lr.endWidth = 0.2f;
        lr.positionCount = 2;
        lr.SetPositions(new Vector3[] { aStart.position, aEnd.position });
        obj = new GameObject();
        obj.transform.SetParent(this.gameObject.transform);
        lr = obj.AddComponent<LineRenderer>();
        lr.startWidth = lr.endWidth = 0.2f;
        lr.positionCount = 2;
        lr.SetPositions(new Vector3[] { bStart.position, bEnd.position });
    }

    private void Start()
    {
        IsIntersect();
    }

    private bool IsIntersect()
    {
        LineSegment a = new LineSegment(aStart.position, aEnd.position);
        LineSegment b = new LineSegment(bStart.position, bEnd.position);
        Debug.Log(a.IsIntersect(b));
        return true;
    }

    private void OnDrawGizmos()
    {
        if (aStart != null && aEnd != null && bStart != null && bEnd != null)
        {
            Gizmos.DrawLine(aStart.position, aEnd.position);
            Gizmos.DrawLine(bStart.position, bEnd.position);
        }
    }
}
