using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FixPhysics
{
    /// <summary> 碰撞检测器 </summary>
    public class CollisionDetector
    {
        float Rxx = 0;
        float Rxy = 0;
        float Rxz = 0;
        float Ryx = 0;
        float Ryy = 0;
        float Ryz = 0;
        float Rzx = 0;
        float Rzy = 0;
        float Rzz = 0;
        float TAz = 0;
        float TAy = 0;
        float TAx = 0;

        public bool DetectCollision(ColliderBox a, ColliderBox b)
        {
            float projection;
            float tProjection;
            bool res = true;
            // T: BoxA BoxB中心点距离/向量
            FixVector3 t = (a.center - b.center);
            for (int i = 1; i <= 15; ++i)
            {
                // 投影
                projection = CaculateProjection(i, a, b);
                // T · L :点积 中心点向量 与 分离轴
                tProjection = CaculateTP(i, t, a, b);
                // T·L > 投影 => 不可能发生碰撞
                if (tProjection > projection)
                {
                    res = false;
                    break;
                }
            }
            return res;
        }
        private float Dot(FixVector3 a, FixVector3 b) => a.Dot(b);
        private float AbsProject(FixVector3 v, FixVector3 dir) => v.AbsProject(dir);
        private float CaculateProjection(int index, ColliderBox a, ColliderBox b)
        {
            float projection = 0;
            switch (index)
            {
                case 1://Ax
                    Rxx = Dot(a.XAxis, b.XAxis);
                    Rxy = Dot(a.XAxis, b.YAxis);
                    Rxz = Dot(a.XAxis, b.ZAxis);
                    projection = a.halfSize.x + Math.Abs(b.halfSize.x * Rxx)
                        + Math.Abs(b.halfSize.y * Rxy) + Math.Abs(b.halfSize.z * Rxz);
                    break;
                case 2://Ay
                    Ryx = Dot(a.YAxis, b.XAxis);
                    Ryy = Dot(a.YAxis, b.YAxis);
                    Ryz = Dot(a.YAxis, b.ZAxis);
                    projection = a.halfSize.y + Math.Abs(b.halfSize.x * Ryx)
                        + Math.Abs(b.halfSize.y * Ryy) + Math.Abs(b.halfSize.z * Ryz);
                    break;
                case 3://Az
                    Rzx = Dot(a.ZAxis, b.XAxis);
                    Rzy = Dot(a.ZAxis, b.YAxis);
                    Rzz = Dot(a.ZAxis, b.ZAxis);
                    projection = a.halfSize.z + Math.Abs(b.halfSize.x * Rzx)
                        + Math.Abs(b.halfSize.y * Rzy) + Math.Abs(b.halfSize.z * Rzz);
                    break;
                case 4://Bx
                    projection = Math.Abs(a.halfSize.x * Rxx) + Math.Abs(a.halfSize.y * Ryx)
                        + Math.Abs(a.halfSize.z * Rzx) + b.halfSize.x;
                    break;
                case 5://By
                    projection = Math.Abs(a.halfSize.x * Rxy) + Math.Abs(a.halfSize.y * Ryy)
                        + Math.Abs(a.halfSize.z * Rzy) + b.halfSize.y;
                    break;
                case 6://Bz
                    projection = Math.Abs(a.halfSize.x * Rxz) + Math.Abs(a.halfSize.y * Ryz)
                        + Math.Abs(a.halfSize.z * Rzz) + b.halfSize.z;
                    break;
                case 7:// Ax x Bx
                    projection = Math.Abs(a.halfSize.y * Rzx) + Math.Abs(a.halfSize.z * Ryx)
                        + Math.Abs(b.halfSize.y * Rxz) + Math.Abs(b.halfSize.z * Rxy);
                    break;
                case 8://Ax x By
                    projection = Math.Abs(a.halfSize.y * Rzy) + Math.Abs(a.halfSize.z * Ryy)
                        + Math.Abs(b.halfSize.x * Rxz) + Math.Abs(b.halfSize.z * Rxx);
                    break;
                case 9://Ax x Bz
                    projection = Math.Abs(a.halfSize.y * Rzz) + Math.Abs(a.halfSize.z * Ryz)
                        + Math.Abs(b.halfSize.x * Rxy) + Math.Abs(b.halfSize.y * Rxx);
                    break;
                case 10://Ay x Bx
                    projection = Math.Abs(a.halfSize.x * Rzx) + Math.Abs(a.halfSize.z * Rxx)
                        + Math.Abs(b.halfSize.y * Ryz) + Math.Abs(b.halfSize.z * Ryy);
                    break;
                case 11://Ay x By
                    projection = Math.Abs(a.halfSize.x * Rzy) + Math.Abs(a.halfSize.z * Rxy)
                        + Math.Abs(b.halfSize.x * Ryz) + Math.Abs(b.halfSize.z * Ryx);
                    break;
                case 12://Ay x Bz
                    projection = Math.Abs(a.halfSize.x * Rzz) + Math.Abs(a.halfSize.z * Rxz)
                        + Math.Abs(b.halfSize.x * Ryy) + Math.Abs(b.halfSize.y * Ryx);
                    break;
                case 13://Az x Bx
                    projection = Math.Abs(a.halfSize.x * Ryx) + Math.Abs(a.halfSize.y * Rxx)
                        + Math.Abs(b.halfSize.y * Rzz) + Math.Abs(b.halfSize.z * Rzy);
                    break;
                case 14://Az x By
                    projection = Math.Abs(a.halfSize.x * Ryy) + Math.Abs(a.halfSize.y * Rxy)
                        + Math.Abs(b.halfSize.x * Rzz) + Math.Abs(b.halfSize.z * Rzx);
                    break;
                case 15://Az x Bz
                    projection = Math.Abs(a.halfSize.x * Ryz) + Math.Abs(a.halfSize.y * Rxz)
                        + Math.Abs(b.halfSize.x * Rzy) + Math.Abs(b.halfSize.y * Rzx);
                    break;

            }
            return projection;
        }
        private float CaculateTP(int index, Vector3 t, ColliderBox a, ColliderBox b)
        {
            float projection = 0;
            switch (index)
            {
                case 1:
                    projection = AbsProject(t, a.XAxis);
                    break;
                case 2:
                    projection = AbsProject(t, a.YAxis);
                    break;
                case 3:
                    projection = AbsProject(t, a.ZAxis);
                    break;
                case 4:
                    projection = AbsProject(t, b.XAxis);
                    break;
                case 5:
                    projection = AbsProject(t, b.YAxis);
                    break;
                case 6:
                    projection = AbsProject(t, b.ZAxis);
                    break;
                case 7://Ax x Bx
                    TAz = Dot(t, a.ZAxis);
                    TAy = Dot(t, a.YAxis);
                    projection = Math.Abs(TAz * Ryx - TAy * Rzx);
                    break;
                case 8://Ax x By
                    projection = Math.Abs(TAz * Ryy - TAy * Rzy);
                    break;
                case 9://Ax x Bz
                    projection = Math.Abs(TAz * Ryz - TAy * Rzz);
                    break;
                case 10://Ay x Bx
                    TAx = Dot(t, a.XAxis);
                    projection = Math.Abs(TAx * Rzx - TAz * Rxx);
                    break;
                case 11://Ay x By
                    projection = Math.Abs(TAx * Rzy - TAz * Rxy);
                    break;
                case 12://Ay x Bz
                    projection = Math.Abs(TAx * Rzz - TAz * Rxz);
                    break;
                case 13://Az x Bx
                    projection = Math.Abs(TAy * Rxx - TAx * Ryx);
                    break;
                case 14://Az x By
                    projection = Math.Abs(TAy * Rxy - TAx * Ryy);
                    break;
                case 15://Az x Bz
                    projection = Math.Abs(TAy * Rxz - TAx * Ryz);
                    break;
            }
            return projection;
        }
    }
}
