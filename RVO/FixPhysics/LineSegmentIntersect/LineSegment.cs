// https://blog.csdn.net/u012861978/article/details/131649350
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LineSegment 
{
    public Vector2 startPos;
    public Vector2 endPos;

    public Vector2 Vector => endPos - startPos;

    public LineSegment(Vector3 startPos, Vector3 endPos)
        : this(new Vector2(startPos.x, startPos.z), new Vector2(endPos.x, endPos.z))
    { }
    public LineSegment(Vector2 startPos, Vector2 endPos)
    { 
        this.startPos = startPos;
        this.endPos = endPos;
    }

    public float Cross(Vector2 a, Vector2 b) => a.x * b.y - a.y * b.x;
    public float Cross(LineSegment other) => Cross(this.Vector, other.Vector);

    public bool IsIntersect(LineSegment other)
    { 
        float cross = Cross(other);
        // 二维叉积不等于0
        // 不平行,有相交可能
        if (cross != 0)
        {
            // 线段AB CD
            // CD两点在AB左右
            Vector2 s2os = other.startPos - this.startPos;
            Vector2 s2oe = other.endPos - this.startPos;
            Vector2 v = Vector;
            // AB两点在CD左右
            Vector2 os2s = this.startPos - other.startPos;
            Vector2 os2e = this.endPos - other.startPos;
            Vector2 ov = other.Vector;
            // 满足条件则证明相交
            return Cross(v, s2os) * Cross(v, s2oe) <= 0 &&
                Cross(ov, os2s) * Cross(ov, os2e) <= 0;
        }
        // 平行,有重合可能
        else
        {
            // 重合未实现,实现看置顶文章
            return false;
        }
    }
}
