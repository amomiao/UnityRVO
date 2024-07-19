using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

namespace FixPhysics
{
    public struct FixVector3
    {
        public static FixVector3 zero => Vector3.zero;

        #region 本类&本类运算符重载
        public static FixVector3 operator +(FixVector3 a, FixVector3 b) => new FixVector3(a.Value + b.Value);
        public static FixVector3 operator -(FixVector3 a, FixVector3 b) => new FixVector3(a.Value - b.Value);
        #endregion 本类&本类运算符重载

        #region 本类&他类运算符重载
        public static FixVector3 operator *(FixVector3 a, FixNum mul) => new FixVector3(a.Value * mul.Value);
        public static FixVector3 operator /(FixVector3 a, FixNum div) => new FixVector3(a.Value / div.Value);
        #endregion 本类&他类运算符重载

        #region 隐式转换
        // 隐式转换：Vector3 与 FixVector3
        public static implicit operator FixVector3(Vector3 vector) => new FixVector3(vector.x, vector.y, vector.z);
        public static implicit operator Vector3(FixVector3 vector) => vector.Value;
        #endregion 隐式转换

        public readonly FixNum x;
        public readonly FixNum y;
        public readonly FixNum z;

        public readonly Vector3 Value => new Vector3(x, y, z);

        public FixVector3(FixNum x, FixNum y, FixNum z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public FixVector3(FixVector3 vector3) : this(vector3.x, vector3.y, vector3.z) { }

        public override string ToString() => Value.ToString();

        /// <summary> 
        /// 向量点积:值为投影长度;
        /// 点积>0 向量夹角 <90,
        /// 点积=0 向量夹角 =90,
        /// 点积<0 向量夹角 >90
        /// </summary>
        public float Dot(FixVector3 other) => x * other.x + y * other.y + z * other.z;
        /// <summary> 点积即投影 </summary>
        public float Project(FixVector3 dir) => Dot(dir);
        /// <summary> 点积/投影的绝对值 </summary>
        public float AbsProject(FixVector3 dir)
        {
            float p = Project(dir);
            return p > 0 ? p : -p;
        }
    }
}
