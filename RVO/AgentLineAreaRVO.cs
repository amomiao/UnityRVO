using FixPhysics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RVO
{
    /// <summary>
    /// 多实体避障的情况下,使用线性规划解决问题
    /// </summary>
    public struct AgentLineAreaRVO
    {
        public readonly FixVector3 originalVector;
        public readonly FixVector3 originalPoint;
        public readonly FixNum a;
        public readonly FixNum b;
        public readonly FixNum c;

        /// <summary> 点斜式求方程 </summary>
        /// <param name="vector"> 仅用于求斜率 </param>
        /// <param name="point"> 过点 </param>
        public AgentLineAreaRVO(FixVector3 vector, FixVector3 point)
        {
            this.originalVector = vector;
            this.originalPoint = point;
            a = vector.z;
            b = -vector.x;
            c = vector.x * point.z - vector.z * point.x;
        }

        /// <summary> 点是否可以相应代理线区域内避障 </summary>
        public bool IsLegal(FixVector3 point)
        {
            return a * point.x + b * point.z + c <= 0;
        }

        public override string ToString() => $"直线公式: {a}X+{b}Y+{c}=0";
    }
}
