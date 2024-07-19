using FixPhysics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RVO
{
    /// <summary>
    /// 矩形处理一个实体的RVO代理:
    /// 仅能处理矩形障碍物
    /// </summary>
    public class RectObsoleteAgentRVO : AgentRVO,IEnumerable
    {
        public struct Rect
        {
            public float width;
            public float height;
            public float DiagonalLineLength => Mathf.Sqrt(width * width + height * height);
            public Rect(float width, float height)
            {
                this.width = width;
                this.height = height;
            }
        }

        private Rect rect;
        private float yRotation;
        private LineSegment a;
        private LineSegment b;
        private LineSegment c;
        private LineSegment d;


        private Vector2 LeftUpPoint => new Vector2(Position.x - rect.width / 2, Position.z + rect.height / 2);
        private Vector2 LeftDownPoint => new Vector2(Position.x - rect.width / 2, Position.z - rect.height / 2);
        private Vector2 RightUpPoint => new Vector2(Position.x + rect.width / 2, Position.z + rect.height / 2);
        private Vector2 RightDownPoint => new Vector2(Position.x + rect.width / 2, Position.z - rect.height / 2);

        public RectObsoleteAgentRVO(FixVector3 startPos, FixNum velocity, FixVector3 directionVector,Rect rect,float yRotation) :
            base(startPos, velocity, directionVector, rect.DiagonalLineLength / 2)
        {
            this.rect = rect;
            this.yRotation = yRotation;
            a = new LineSegment(LeftUpPoint,LeftDownPoint);
            b = new LineSegment(LeftUpPoint,RightUpPoint);
            c = new LineSegment(RightDownPoint,LeftDownPoint);
            d = new LineSegment(RightDownPoint,RightUpPoint);
        }

        ///// <summary> 静物不具有速度 </summary>
        //public override void UpdateSpeed(FixNum velocity) { }
        ///// <summary> 静物不具有移动方向 </summary>
        //public override void UpdateDirectionVector(FixVector3 directionVector) { }

        protected override E_State IsSureOtherCollisionSelf(FixVector3 relativeIncre, FixNum exapndR, FixNum tangleLength, FixNum halfAngle, AgentRVO requester)
        {
            // 增量点
            FixVector3 point = requester.Position + relativeIncre;
            FixNum angle = Vector3.SignedAngle(OtherToThisVector(requester), point, Vector3.up);
            //// 1.三角外 不会发生碰撞
            //if (Mathf.Abs(angle) > halfAngle)
            //    return E_State.No;
            // 在小圆外并且在大圆内 不会发生碰撞
            if (
                (point - this.Position).Value.sqrMagnitude > exapndR * exapndR &&
                 relativeIncre.Value.sqrMagnitude <= tangleLength * tangleLength)
                return E_State.No;
            else
            {
                int count = 0;
                LineSegment l = new LineSegment(requester.Position, point.Value + requester.Radius.Value * relativeIncre.Value.normalized);
                foreach (LineSegment line in this)
                {
                    // 若线段与任意一条边相交,则将碰撞
                    if (l.IsIntersect(line))
                    {
                        Debug.Log(count);
                        return E_State.Inner;
                    }
                    count++;
                }
            }
            return E_State.No;
        }

        public IEnumerator GetEnumerator()
        {
            yield return a;
            yield return b;
            yield return c;
            yield return d;
        }
    }
}